using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using BlogReader.Views;
using System.Windows.Input;
using MvvmHelpers;
using BlogReaderSharedLibrary.DTOs;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using BlogReader.Models;
using System.Linq;
using BlogReader.Services;
using MvvmHelpers.Commands;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;

namespace BlogReader.ViewModels
{
    public class FavoritesPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Articles> Articles { get; set; }
        public ICommand GetArticles { get; }
        public ICommand SettingsButton { get; }
        public ICommand DeleteArticle { get; }
        public ICommand ReadArticle { get; }
        private bool OfflineMode { get; set; }
        private readonly IArticleRepository _articleRepository;

        public FavoritesPageViewModel()
        {
            _articleRepository = DependencyService.Get<IArticleRepository>();
            GetArticles = new AsyncCommand(async () => await GetArticlesAsync());
            SettingsButton = new AsyncCommand(async () => await GoToSettingsPageAsync());
            DeleteArticle = new AsyncCommand<Articles>(async (article) => await DeleteArticleFromDB(article));
            ReadArticle = new AsyncCommand<Articles>(async (article) => await ReadSelectedArticle(article));
            Task.Run(async () => await GetArticlesAsync());
        }

        private async Task ReadSelectedArticle(Articles article)
        {
            if (OfflineMode)
            {
                await Application.Current.MainPage.Navigation.PushAsync(new ArticleReadPage(article));
            }
            else
            {
                await OpenBrowser(article.RootLink);
            }
        }

        private async Task OpenBrowser(string rootLink)
        {
            try
            {
                await Browser.OpenAsync(rootLink, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                await Device.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.DisplayAlert("Error", $"Something went wrong opening the browser :(", "OK"));
            }
        }

        private async Task DeleteArticleFromDB(Articles article)
        {
            Articles.Remove(article);
            if (article.ForOffline)
            {
                article.Favorite = !article.Favorite;
                await _articleRepository.UpdateArticle(article);
            }
            else
            {
                await _articleRepository.DeleteArticleAsync(article.RealArticleID);
            }
            await Application.Current.MainPage.DisplayToastAsync("Article removed from favorites", 3000);
        }

        private async Task GoToSettingsPageAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new SettingsPage());
        }

        private async Task GetArticlesAsync()
        {
            CheckNetworkConnection();

            List<Articles> favoriteArticles = await _articleRepository.GetFavoriteArticlesAsync();
            Articles = new ObservableCollection<Articles>(favoriteArticles.Where(a => favoriteArticles.Any(a2 => a2.RealArticleID == a.RealArticleID)));
        }
        private bool CheckNetworkConnection()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                OfflineMode = true;
                return false;
            }
            OfflineMode = false;
            return true;
        }
    }
}
