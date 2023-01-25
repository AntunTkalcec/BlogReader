using BlogReader.Views;
using Plugin.FirebasePushNotification;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BlogReader.Services
{
    public static class NotificationManager
    {
        public static Task ManageNotification(object s, FirebasePushNotificationResponseEventArgs p)
        {
            var article = p.Data["RootLink"].ToString();

            Application.Current.MainPage = new NavigationPage(new ArticlesPage(article));
            return Task.CompletedTask;
        }
    }
}
