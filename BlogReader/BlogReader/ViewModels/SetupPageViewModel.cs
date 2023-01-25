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
using MonkeyCache.FileStore;
using System.Collections.Specialized;
using Xamarin.CommunityToolkit.Extensions;

namespace BlogReader.ViewModels
{
    public class SetupPageViewModel : INotifyPropertyChanged
    {
        public ICommand GetBlogs { get; }
        public ICommand SaveBlogs { get; }
        public ICommand DeleteBlogs { get; }
        public bool NextEnabled { get; set; } = false;
        public bool IsRefreshing { get; set; }

        private readonly IBlogRepository _blogRepository;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Blogs> Blogs { get; set; }
        public ObservableCollection<object> SelectedBlogs { get; set; } = new ObservableCollection<object>();
        public SetupPageViewModel()
        {
            _blogRepository = DependencyService.Get<IBlogRepository>();
            GetBlogs = new AsyncCommand(async () => await GetBlogsAsync());
            SaveBlogs = new AsyncCommand(async () => await SaveBlogsToDB());
            DeleteBlogs = new AsyncCommand(async () => await DeleteBlogsDB());
            Task.Run(async () => await GetBlogsAsync());
        }

        private async Task DeleteBlogsDB()
        {
            await _blogRepository.DeleteAllBlogsAsync();
            await Application.Current.MainPage.DisplayAlert("Testing", $"Blogs have been deleted from the DB.", "OK");
            Application.Current.MainPage = new NavigationPage(new LandingPage());
        }

        private async Task SaveBlogsToDB()
        {
             Barrel.Current.EmptyAll();
             List<Models.Blogs> blogs = new();
             var lista = SelectedBlogs.Select(s => s as Models.Blogs).ToList();
             await _blogRepository.CreateNewBlogsAsync(lista);
             await Application.Current.MainPage.Navigation.PopToRootAsync();
             Application.Current.MainPage = new NavigationPage(new ArticlesPage());
        }

        private async Task GetBlogsAsync()
        {
            IsRefreshing = true;
            try
            {
                HttpClient client = new(GetInsecureHandler());
                var response = await client.GetAsync($"{SettingsManager.BaseURL}/v1/blogs/");
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var nekaj = JsonConvert.DeserializeObject<List<BlogDTO>>(jsonResponse);
                Blogs = new ObservableCollection<Blogs>(nekaj.Select(b => b.FromDTO()));
                List<Blogs> blogsFromDB = await _blogRepository.GetBlogsAsync();
                SelectedBlogs = new ObservableCollection<object>(Blogs.Where(b => blogsFromDB.Any(b2 => b2.RealBlogID == b.RealBlogID)));
            }
            catch (Exception ex)
            {
                await Device.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.DisplayAlert("Error",
                    $"Something went wrong getting the blogs :( Check your internet connection.", "OK"));
            }
            IsRefreshing = false;
        }
        public HttpClientHandler GetInsecureHandler()
        {
            HttpClientHandler handler = new HttpClientHandler
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
