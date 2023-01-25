using BlogReaderCoreLibrary.Entities;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using System.Reflection;

namespace BlogReaderCoreSharedLibrary.FirebaseNotifications
{
    public static class FCM
    {
        public static void Init ()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var path = assembly.GetManifestResourceNames().Single(n => n.EndsWith("private_key.json"));
            using var st = assembly.GetManifestResourceStream(path);
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromStream(st)
            });
        }
        /// <summary>
        /// Programatically sends a notification to all devices subscribed to the given topic.
        /// </summary>
        /// <param name="topic">topic to send notification to</param>
        /// <param name="notificationTitle">title of the notification</param>
        /// <param name="notificationBody">body of the notification</param>
        /// <param name=""></param>
        /// <returns></returns>
        public static async Task<string> SendNotificationAsync(string topic, string notificationTitle, string notificationBody, Article article)
        {
            var message = new Message()
            {
                Data = new Dictionary<string, string>()
                {
                    { "body", $"{notificationBody}" },
                    { "title", $"{notificationTitle}" },
                    { "ArticleID", $"{article.ArticleID}" },
                    { "RootLink", $"{article.RootLink}" },
                    { "icon", "ekobitlogo" },
                    { "content_available", "true" },
                },
                Android = new AndroidConfig() { Priority = Priority.High },
                Topic = topic,    
            };

            return await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }
    }
}
