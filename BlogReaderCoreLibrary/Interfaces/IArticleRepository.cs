using BlogReaderCoreLibrary.Entities;

namespace BlogReaderCoreLibrary.Interfaces
{
    public interface IArticleRepository
    {
        Task<List<Article>> GetArticlesAsync();
        Task<List<Article>> GetArticlesAsync(int id);
        Task<List<Article>> GetArticlesAsync(DateTime timeFrom, DateTime timeTo);
        Task<List<Article>> GetArticlesAsync(string category);
        Task<List<Article>> GetArticlesByTermAsync(string term);
        Task<List<Article>> GetArticlesFromBlogsAsync(string blogs);
        Task<List<Article>> GetArticlesFromBlogsAsync(string blogs, DateTime timeFrom, DateTime timeTo);
        Task<List<Article>> GetArticlesFromSourceAsync(int sourceID);
        Task<List<Article>> GetArticlesFromDBAsync(List<Article> articles);
        Task<Article> GetArticleAsync(int id);
        bool ArticleExists(int id);
        bool ArticleExists(string name);
        bool CreateArticle(Article article);
        bool UpdateArticle(Article article);
        bool DeleteArticle(Article article);
        bool Save();
    }
}
