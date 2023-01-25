using BlogReader.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BlogReader.Services
{
    public class SourceRepository
    {
        SQLiteAsyncConnection conn;
        public SourceRepository(string dbPath)
        {
            conn = new SQLiteAsyncConnection(dbPath);
            conn.CreateTableAsync<Sources>().Wait();
        }
        public async Task CreateNewSourceAsync(Sources source)
        {
            try
            {
                await conn.InsertAsync(source);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Something went wrong there :( Try again or contact support. Here's the full error: {ex}", "OK");
            }
        }
        public async Task<List<Sources>> GetSourcesAsync()
        {
            try
            {
                return await conn.Table<Sources>().ToListAsync();
            }
            catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Something went wrong there :( Try again or contact support. Here's the full error: {ex}", "OK");
            }
            return new List<Sources>();
        }
        public async Task DeleteAllSourcesAsync()
        {
            try
            {
                _ = await conn.DeleteAllAsync<Sources>();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Something went wrong deleting stuff :( Try again or contact support. Here's the full error: {ex}", "OK");
            }
        }
        public async Task DeleteSourceAsync(int id)
        {
            try
            {
                _ = await conn.DeleteAsync<Sources>(id);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Something went wrong deleting that :( Try again or contact support. Here's the full error: {ex}", "OK");
            }
        }
        public async Task CreateNewSourcesAsync(List<Sources> sources)
        {
            try
            {
                _ = await conn.InsertAllAsync(sources);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Something went wrong there :( Try again or contact support. Here's the full error: {ex}", "OK");
            }
        }
    }
}
