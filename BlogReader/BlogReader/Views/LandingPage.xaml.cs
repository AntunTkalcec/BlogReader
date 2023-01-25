using BlogReader.Resources;
using Plugin.FirebasePushNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BlogReader.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LandingPage : ContentPage
    {
        public LandingPage()
        {
            InitializeComponent();
            if (Preferences.Get("language", "en-US" == "en-US"))
            {
                EnglishFrame.BackgroundColor = Color.FromHex("#99E097");
                CroatianFrame.BackgroundColor = Color.WhiteSmoke;
            }
            else
            {
                EnglishFrame.BackgroundColor = Color.WhiteSmoke;
                CroatianFrame.BackgroundColor = Color.FromHex("#99E097");
            }
        }

        protected override void OnAppearing()
        {
            AnimateItemsAsync();
        }
        private async Task AnimateItemsAsync()
        {
            await animationView.ScaleTo(1, 750, Easing.Linear);
            await LandingPageTitle.TranslateTo(0, 0, 350, Easing.Linear);
            await BeginShadows.TranslateTo(0, 0, 300, Easing.Linear);
            await AboutButton.TranslateTo(0, 0, 250, Easing.Linear);
            await BottomStackLayout.TranslateTo(0, 0, 250, Easing.Linear);
        }

        private void EnglishFrameTapped(object sender, EventArgs e)
        {
            EnglishFrame.BackgroundColor = Color.FromHex("#99E097");
            CroatianFrame.BackgroundColor = Color.WhiteSmoke;
        }

        private void CroatianFrameTapped(object sender, EventArgs e)
        {
            CroatianFrame.BackgroundColor = Color.FromHex("#99E097");
            EnglishFrame.BackgroundColor = Color.WhiteSmoke;
        }
    }
}