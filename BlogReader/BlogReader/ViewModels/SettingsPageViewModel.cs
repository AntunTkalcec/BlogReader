using BlogReader.Views;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BlogReader.ViewModels
{
    public class SettingsPageViewModel
    {
        public ICommand RepeatSetupButton { get; }
        public ICommand ShowOfflineArticles { get; }
        public ICommand OpenFavoriteArticles { get; }
        public ICommand AboutPage { get; }
        public ICommand ChangeToEng { get; }
        public ICommand ChangeToCro { get; }
        public SettingsPageViewModel()
        {
            RepeatSetupButton = new AsyncCommand(async () => await RepeatSetup());
            ShowOfflineArticles = new AsyncCommand(async () => await ShowSavedArticles());
            OpenFavoriteArticles = new AsyncCommand(async () => await FavoriteArticles());
            ChangeToCro = new Xamarin.Forms.Command(SetLangCro);
            ChangeToEng = new Xamarin.Forms.Command(SetLangEng);
            AboutPage = new AsyncCommand(async () => await GoToAboutPage());
        }

        private async Task GoToAboutPage()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AboutPage());
        }

        private void SetLangEng(object obj)
        {
            LocalizationResourceManager.Current.CurrentCulture = new CultureInfo("en-US");
            Preferences.Set("language", "en-US");
        }

        private void SetLangCro(object obj)
        {
            LocalizationResourceManager.Current.CurrentCulture = new CultureInfo("hr-HR");
            Preferences.Set("language", "hr-HR");
        }

        private async Task FavoriteArticles()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new FavoritesPage());
        }

        private async Task ShowSavedArticles()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new OfflineArticlesPage());
        }

        private async Task RepeatSetup()
        {
            await Application.Current.MainPage.Navigation.PopToRootAsync(false);
            await Application.Current.MainPage.Navigation.PushAsync(new SetupPage());
        }
    }
}
