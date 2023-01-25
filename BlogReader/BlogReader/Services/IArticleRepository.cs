using BlogReader.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogReader.Services
{
    public interface IArticleRepository
    {
        Task CreateNewArticleAsync(Articles article);
        Task CreateNewArticlesAsync(List<Articles> article);
        Task DeleteAllArticlesAsync();
        Task DeleteArticleAsync(int id);
        Task<List<Articles>> GetArticlesAsync();
        Task<List<Articles>> GetFavoriteArticlesAsync();
        Task UpdateArticle(Articles article);
        Task<Articles> GetArticle(int id);
    }
}