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

namespace BlogReader.ViewModels
{
    public class OfflineArticlesPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Articles> Articles { get; set; }
        public ICommand GetArticles { get; }
        public ICommand SettingsButton { get; }
        public ICommand DeleteArticle { get; }
        public ICommand ReadArticle { get; }
        private readonly IArticleRepository _articleRepository;

        public OfflineArticlesPageViewModel()
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
            await Application.Current.MainPage.Navigation.PushAsync(new ArticleReadPage(article));
        }

        private async Task DeleteArticleFromDB(Articles article)
        {
            Articles.Remove(article);
            if (article.Favorite)
            {
                article.ForOffline = !article.ForOffline;
                await _articleRepository.UpdateArticle(article);
            }
            else
            {
                await _articleRepository.DeleteArticleAsync(article.RealArticleID);
            }
            await Application.Current.MainPage.DisplayToastAsync("Article deleted.", 3000);
        }

        private async Task GoToSettingsPageAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new SettingsPage());
        }

        private async Task GetArticlesAsync()
        {
            List<Articles> articles = await _articleRepository.GetArticlesAsync();
            Articles = new ObservableCollection<Articles>(articles.Where(a => articles.Any(a2 => a2.RealArticleID == a.RealArticleID)).Where(a3 => a3.ForOffline 
            == true));
        }
    }
}
