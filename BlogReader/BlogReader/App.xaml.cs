using BlogReader.Models;
using BlogReader.Resources;
using BlogReader.Services;
using BlogReader.Views;
using MonkeyCache.FileStore;
using Plugin.FirebasePushNotification;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: ExportFont("FontAwesome6Brands.otf", Alias = "FABrands")]
[assembly: ExportFont("FontAwesome6Regular.otf", Alias = "FARegular")]
[assembly: ExportFont("FontAwesome6Solid.otf", Alias = "FASolid")]
[assembly: ExportFont("Jersey.ttf", Alias = "Momcake")]
[assembly: ExportFont("Courgette-Regular.ttf", Alias = "Courgette")]

namespace BlogReader
{
    public partial class App : Application
    {
        private readonly IBlogRepository _blogRepository;
        public App()
        {
            InitializeComponent();
            _blogRepository = DependencyService.Get<IBlogRepository>();           
            Barrel.ApplicationId = AppInfo.PackageName;
            Sharpnado.Shades.Initializer.Initialize(loggerEnable: false);
            LocalizationResourceManager.Current.PropertyChanged += (_, _) => AppResources.Culture = LocalizationResourceManager.Current.CurrentCulture;
            LocalizationResourceManager.Current.Init(AppResources.ResourceManager);
            LocalizationResourceManager.Current.CurrentCulture = new System.Globalization.CultureInfo($"{Preferences.Get("language", "en-US")}");
            MainPage = new EmptyPage();
            CrossFirebasePushNotification.Current.OnNotificationReceived += async (s, p) =>
            {
                p.Data.TryGetValue("body", out object message);
                await Device.InvokeOnMainThreadAsync(async () =>
                    await Xamarin.Forms.Application.Current.MainPage.DisplayToastAsync($"{message}"));
            };
            CrossFirebasePushNotification.Current.OnNotificationOpened += async (s, p) => await NotificationManager.ManageNotification(s, p);
        }

        private bool CheckNetworkConnection()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                return false;
            }
            return true;
        }

        protected override async void OnStart()
        {
            //check setup to determine start page
            List<Blogs> blogs = await _blogRepository.GetBlogsAsync();
            if (blogs.Count > 0 && CheckNetworkConnection())
            {
                MainPage = new NavigationPage(new ArticlesPage());
            }
            else if (blogs.Count == 0)
            {
                MainPage = new NavigationPage(new LandingPage());
            }
            //end check setup to determine start page
            CrossFirebasePushNotification.Current.Subscribe("General");
            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine($"Token: {p.Token}");
            };
            
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
