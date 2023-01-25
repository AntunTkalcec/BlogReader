using BlogReader.Models;
using BlogReader.Resources;
using BlogReader.Services;
using BlogReader.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace BlogReader.ViewModels
{
    public class LandingPageViewModel : ObservableObject
    {
        public ICommand AboutIconClick { get; }
        public ICommand BeginClick { get; }
        public ICommand SetLangEnglish { get; }
        public ICommand SetLangCroatian { get; }
        private ICommand CheckSetup { get; set; }
        private readonly IBlogRepository _blogRepository;
        public LandingPageViewModel()
        {
            _blogRepository = DependencyService.Get<IBlogRepository>(); 
            AboutIconClick = new AsyncCommand(async () => await GoToAboutPage());
            BeginClick = new AsyncCommand(async () => await CheckSetupComplete());
            SetLangCroatian = new Xamarin.Forms.Command(SetLangCro);
            SetLangEnglish = new Xamarin.Forms.Command(SetLangEng);
            CheckSetup = new AsyncCommand(async () => await CheckSetupComplete());
        }

        private void SetLangEng(object obj)
        {
            LocalizationResourceManager.Current.CurrentCulture = new CultureInfo("en-US");
            Preferences.Set("language", "en-US");
        }

        private void SetLangCro()
        {            
            LocalizationResourceManager.Current.CurrentCulture = new CultureInfo("hr-HR");
            Preferences.Set("language", "hr-HR");
        }

        private async Task GoToSetupPage()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new SetupPage());
        }

        private bool CheckNetworkConnection()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                return false;
            }
            else if (Connectivity.NetworkAccess == NetworkAccess.Unknown)
            {
                return false;
            }
            return true;
        }

        private async Task GoToAboutPage()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AboutPage());
        }
        private async Task CheckSetupComplete()
        {
            List<Blogs> blogs = await _blogRepository.GetBlogsAsync();
            if (blogs.Count > 0 && CheckNetworkConnection())
            {
                GoToArticlesPage();
            }
            else if (blogs.Count == 0 && CheckNetworkConnection())
            {
                await GoToSetupPage();
            }
            else if (blogs.Count == 0 && !CheckNetworkConnection())
            {
                await Device.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.DisplayAlert("Warning", 
                    "No internet connection detected.", "OK"));
            }
            else if (blogs.Count > 0 && !CheckNetworkConnection())
            {
                await GoToOfflineArticlesPage();
            }
        }

        private void GoToArticlesPage()
        {
            Application.Current.MainPage = new NavigationPage(new ArticlesPage());
        }
        private async Task GoToOfflineArticlesPage()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new OfflineArticlesPage());
        }
    }
}
