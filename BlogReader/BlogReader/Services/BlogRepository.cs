using BlogReader.Models;
using BlogReader.Services;
using BlogReaderSharedLibrary.DTOs;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(BlogRepository))]
namespace BlogReader.Services
{
    public class BlogRepository : IBlogRepository
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
            await conn.CreateTableAsync<Blogs>();
        }
        public async Task CreateNewBlogAsync(Blogs blog)
        {
            await Init();
            try
            {
                await conn.InsertAsync(blog);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Something went wrong there :( Try again or contact support. Here's the full error: {ex}", "OK");
            }
        }
        public async Task<List<Blogs>> GetBlogsAsync()
        {
            await Init();
            try
            {
                return await conn.Table<Blogs>().ToListAsync();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Something went wrong there :( Try again or contact support. Here's the full error: {ex}", "OK");
            }
            return new List<Blogs>();
        }
        public async Task DeleteAllBlogsAsync()
        {
            await Init();
            try
            {
                _ = await conn.DeleteAllAsync<Blogs>();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Something went wrong deleting stuff :( Try again or contact support. Here's the full error: {ex}", "OK");
            }
        }
        public async Task DeleteBlogAsync(int id)
        {
            await Init();
            try
            {
                _ = await conn.DeleteAsync<Blogs>(id);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Something went wrong deleting that :( Try again or contact support. Here's the full error: {ex}", "OK");
            }
        }
        public async Task CreateNewBlogsAsync(List<Blogs> blogs)
        {
            await Init();
            try
            {
                await DeleteAllBlogsAsync();
                _ = await conn.InsertAllAsync(blogs);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Something went wrong there :( Try again or contact support. Here's the full error: {ex}", "OK");
            }
        }
    }
}
