using BlogReaderCoreLibrary.Entities;
using BlogReaderCoreLibrary.Interfaces;
using BlogReaderCoreSharedLibrary.FirebaseNotifications;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BlogReaderInfrastructure.Jobs
{
    public class JobWorker : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public JobWorker(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            List<Blog> blogs = GetBlogs();
            IRecurringJobManager recurringJobManager = new RecurringJobManager();
            foreach (var blog in blogs)
            {
                recurringJobManager.AddOrUpdate($"GetArticlesFromBlog{blog.ID}Job", () => GetArticles(blog.ID), $"{blog.Cron}");
            }
            return Task.CompletedTask;
        }

        private List<Blog> GetBlogs()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var blogRepository = scope.ServiceProvider.GetRequiredService<IBlogRepository>();
                return blogRepository.GetBlogs().ToList();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task GetArticles(int blogID)
        {
            Console.WriteLine($"Starting job for blog {blogID}");
            using (var scope = _serviceProvider.CreateScope())
            {
                var rssReader = scope.ServiceProvider.GetRequiredService<IRSSreader>();
                var articleRepository = scope.ServiceProvider.GetRequiredService<IArticleRepository>();
                Blog blog = await rssReader.GetBlogAsync(blogID);
                List<Article> articles = await rssReader.GetArticlesAsync(blog);
                List<Article> articlesFromDB = await rssReader.GetArticlesFromDBAsync(articles);

                List<Article> newArticles = articles.ExceptBy(articlesFromDB.Select(x => x.ArticleID), y => y.ArticleID).ToList();

                if (newArticles.Count > 0)
                {
                    foreach (var article in newArticles)
                    {
                        articleRepository.CreateArticle(article);
                    }
                    await FCM.SendNotificationAsync("General", "New article", "There is a new article for you to read.", newArticles.Last());
                }
            }   
        }
    }
}
