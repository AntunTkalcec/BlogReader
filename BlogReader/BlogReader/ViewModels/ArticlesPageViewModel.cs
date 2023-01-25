using BlogReader.Models;
using BlogReader.Services;
using BlogReader.Views;
using BlogReaderSharedLibrary.DTOs;
using MvvmHelpers.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.CommunityToolkit.Extensions;
using MonkeyCache.FileStore;
using System.ComponentModel;

namespace BlogReader.ViewModels
{
    public class ArticlesPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Articles> Articles { get; set; } = new ObservableCollection<Articles>();
        public ICommand GetArticles { get; }
        public ICommand SettingsButton { get; }
        public ICommand SaveArticle { get; }
        public ICommand ReadArticle { get; }
        public ICommand RefreshArticles { get; }
        public ICommand FavoriteArticle { get; }
        public ICommand PerformSearch { get; }
        public bool SavedArticle { get; set; } 
        public bool FavoritedArticle { get; set; } 
        public bool IsRefreshing { get; set; }
        public bool Initialized { get; set; } = false;
        private string BarrelId { get; set; } = "articlesKey";
        private readonly HttpClient client;
        private List<Blogs> blogs;
        private string blogs2get;
        private readonly IBlogRepository _blogRepository;
        private readonly IArticleRepository _articleRepository;
        public ArticlesPageViewModel()
        {
            _blogRepository = DependencyService.Get<IBlogRepository>();
            _articleRepository = DependencyService.Get<IArticleRepository>();
            GetArticles = new AsyncCommand(async () => await GetArticlesAsync());
            SettingsButton = new AsyncCommand(async () => await GoToSettingsPageAsync());
            SaveArticle = new AsyncCommand<Articles>(async (article) => await SaveArticleToDB(article));
            ReadArticle = new AsyncCommand<Articles>(async (article) => await ReadSelectedArticle(article));
            RefreshArticles = new AsyncCommand(async () => await RefreshCollectionView());
            FavoriteArticle = new AsyncCommand<Articles>(async (article) => await SaveArticleToFavorites(article));
            PerformSearch = new AsyncCommand<string>(async (text) => await Search(text));
            client = new(GetInsecureHandler());
            RefreshArticles.Execute(null);
        }

        private async Task Search(string text)
        {
            Articles.Clear();
            if (text == ".")
            {
                await Device.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.DisplayToastAsync("Invalid input."));
                return;
            }
            IsRefreshing = true;
            try
            {
                var jsonResponse = await client.GetStringAsync($"{SettingsManager.BaseURL}/v1/term/{text}/articles");
                Articles = new ObservableCollection<Articles>(JsonConvert.DeserializeObject<List<ArticleDTO>>(jsonResponse).Select(a => a.FromDTOArticles()));
            }
            catch (Exception ex)
            {
                await Device.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.DisplayAlert("Error",
                    $"Something went wrong getting the articles :( Try again or contact support. " +
                    $"Here's the full error: " +
                    $"{ex}", "OK"));
            }
            if (Articles.Count == 0)
            {
                await Device.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.DisplayToastAsync("There are no articles " +
                    "to show."));
            }
            IsRefreshing = false;
        }

        private async Task SaveArticleToFavorites(Articles article)
        {
            Articles art = await _articleRepository.GetArticle(article.RealArticleID);
            if (art != null)
            {
                art.Favorite = !art.Favorite;
                article.Favorite = art.Favorite;
                if (!art.Favorite && !art.ForOffline)
                {
                    await _articleRepository.DeleteArticleAsync(art.RealArticleID);
                }
                else
                {
                    await _articleRepository.UpdateArticle(art);
                }
                await Device.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.DisplayToastAsync("Article updated.", 3000));
            }
            else
            {
                article.Favorite = true;
                await _articleRepository.CreateNewArticleAsync(article);
                await Device.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.DisplayToastAsync("Article saved to Favorites.", 3000));
            }
        }

        public async Task RefreshCollectionView()
        {
            try
            {
                IsRefreshing = true;
                Articles.Clear();
                await GetArticlesAsync();
            }
            catch (Exception)
            {

            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private async Task ReadSelectedArticle(Articles article)
        {
            await OpenBrowser(article.RootLink);
        }

        private async Task SaveArticleToDB(Articles article)
        {
            Articles art = await _articleRepository.GetArticle(article.RealArticleID);
            if (art != null)
            {
                art.ForOffline = !art.ForOffline;
                article.ForOffline = art.ForOffline;
                if (!art.ForOffline && !art.Favorite)
                {
                    await _articleRepository.DeleteArticleAsync(art.RealArticleID);
                }
                else
                {
                    await _articleRepository.UpdateArticle(art);
                }
                await Device.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.DisplayToastAsync("Article updated.", 3000));
            }
            else
            {
                article.ForOffline = true;
                await _articleRepository.CreateNewArticleAsync(article);
                await Device.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.DisplayToastAsync("Article saved for offline reading.", 3000));
            }
        }

        private async Task GoToSettingsPageAsync()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new SettingsPage());
        }

        private async Task GetArticlesAsync()
        {
            blogs = await _blogRepository.GetBlogsAsync();
            blogs2get = string.Join(",", blogs.Select(q => q.RealBlogID).ToList());
            try
            {
                var json = string.Empty;
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    json = Barrel.Current.Get<string>(BarrelId);
                }
                else if (!Barrel.Current.IsExpired(BarrelId))
                {
                    json = Barrel.Current.Get<string>(BarrelId);
                }

                if (string.IsNullOrWhiteSpace(json))
                {
                    json = await client.GetStringAsync($"{SettingsManager.BaseURL}/v1/blogs/{blogs2get}/articles");
                    Barrel.Current.Add(BarrelId, json, TimeSpan.FromMinutes(5));
                }
                Articles = new ObservableCollection<Articles>(JsonConvert.DeserializeObject<List<ArticleDTO>>(json).Select(a => a.FromDTOArticles()).OrderByDescending(ar => 
                ar.PublishDate));
                
                if (Articles.Count > 0)
                {
                    await ChangeNullImages();
                }

                List<Articles> articlesFromDB = await _articleRepository.GetArticlesAsync();
                foreach (var item in Articles)
                {
                    var art = articlesFromDB.SingleOrDefault(x => x.RealArticleID == item.RealArticleID);
                    if (art != null)
                    {
                        item.Favorite = art.Favorite;
                        item.ForOffline = art.ForOffline;
                    }
                }
            }
            catch (Exception ex)
            {
                await Device.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.DisplayAlert("Error",
                    $"Something went wrong getting the articles :( Check your internet connection.", "OK"));
            }
        }

        private Task ChangeNullImages()
        {
            foreach (var article in Articles)
            {
                if (article.ImageURL == String.Empty)
                {
                    article.ImageURL = "https://cdn0.iconfinder.com/data/icons/media-5/512/news_paper-512.png";
                }
            }
            return Task.CompletedTask;
        }

        public async Task OpenBrowser(string uri)
        {
            try
            {
                await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                await Device.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.DisplayAlert("Error", $"Something went wrong opening the browser :(", "OK"));
            }
        }
        private HttpMessageHandler GetInsecureHandler()
        {
            HttpClientHandler handler = new()
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                {
                    if (cert.Issuer.Equals("CN=localhost"))
                        return true;
                    return errors == System.Net.Security.SslPolicyErrors.None;
                }
            };
            return handler;
        }
    }
}
