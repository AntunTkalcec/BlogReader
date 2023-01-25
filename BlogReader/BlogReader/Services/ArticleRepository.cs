using BlogReader.Models;
using BlogReader.Services;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(ArticleRepository))]
namespace BlogReader.Services
{
    public class ArticleRepository : IArticleRepository
    {
        SQLiteAsyncConnection conn;
        async Task Init()
        {
            if (conn != null)
            {
                return;
            }
            var dbPath = FileAccessHelper.GetLocalFilePath("blogreader.db3");
            conn = new SQLiteAsyncConnection(dbPath);
            await conn.CreateTableAsync<Articles>();
        }
        public async Task CreateNewArticleAsync(Articles article)
        {
            await Init();
            try
            {
                await conn.InsertAsync(article);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Something went wrong there :( Try again or contact support. Here's the full error: {ex}", "OK");
            }
        }
        public async Task<List<Articles>> GetArticlesAsync()
        {
            await Init();
            try
            {
                return await conn.Table<Articles>().ToListAsync();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Something went wrong there :( Try again or contact support. Here's the full error: {ex}", "OK");
            }
            return new List<Articles>();
        }
        public async Task DeleteAllArticlesAsync()
        {
            await Init();
            try
            {
                _ = await conn.DeleteAllAsync<Articles>();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Something went wrong deleting stuff :( Try again or contact support. Here's the full error: {ex}", "OK");
            }
        }
        public async Task DeleteArticleAsync(int id)
        {
            await Init();
            try
            {
                Articles article = await conn.Table<Articles>().FirstOrDefaultAsync(x => x.RealArticleID == id);
                _ = await conn.DeleteAsync<Articles>(article.ID);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Something went wrong deleting that :( Try again or contact support. Here's the full error: {ex}", "OK");
            }
        }
        public async Task CreateNewArticlesAsync(List<Articles> article)
        {
            await Init();
            try
            {
                _ = await conn.InsertAllAsync(article);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Something went wrong there :( Try again or contact support. Here's the full error: {ex}", "OK");
            }
        }

        public async Task<List<Articles>> GetFavoriteArticlesAsync()
        {
            await Init();
            try
            {
                List<Articles> allArticles = await conn.Table<Articles>().ToListAsync();
                return allArticles.Where(a => a.Favorite == true).ToList();
            }
            catch(Exception ex)
            {
                await Device.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.DisplayAlert("Error", $"Something went wrong getting your favorite " +
                    $"articles :( Here's the full error: {ex}", "OK"));
            }
            return new List<Articles>();
        }
        public async Task UpdateArticle(Articles article)
        {
            await Init();
            try
            {
                await conn.UpdateAsync(article);
            }
            catch (Exception ex)
            {
                await Device.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.DisplayAlert("Error", $"Something went wrong there" +
                    $":( Here's the full error: {ex}", "OK"));
            }
        }

        public async Task<Articles> GetArticle(int id)
        {
            await Init();
            try
            {
                return await conn.Table<Articles>().FirstOrDefaultAsync(x => x.RealArticleID == id);
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
