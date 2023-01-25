using BlogReaderCoreLibrary.Entities;

namespace BlogReaderCoreLibrary.Interfaces
{
    public interface IRSSreader
    {
        Task<List<Article>> GetArticlesAsync(Blog blog);
        Task<List<Article>> GetArticlesFromDBAsync(List<Article> articles);
        Task<Blog> GetBlogAsync(int blogID);
        Task<List<Article>> GetArticlesAndSaveToDb(int blogId);
    }
}
