using BlogReaderCoreLibrary.Entities;

namespace BlogReaderCoreLibrary.Interfaces
{
    public interface IBlogRepository
    {
        Task<List<Blog>> GetBlogsAsync();
        Task<List<Blog>> GetBlogsFromSourceAsync(int id);
        Task<Blog> GetBlogAsync(int id);
        IQueryable<Blog> GetBlogs();
        Task<List<Blog>> GetBlogsAndSourceAsync();
        bool BlogExists(int id);
        bool BlogExists(string name);
        bool CreateBlog(Blog blog);
        bool UpdateBlog(Blog blog);
        bool DeleteBlog(Blog blog);
        bool Save();
    }
}
