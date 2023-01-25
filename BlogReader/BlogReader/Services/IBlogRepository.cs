using BlogReader.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogReader.Services
{
    public interface IBlogRepository
    {
        Task CreateNewBlogAsync(Blogs blog);
        Task CreateNewBlogsAsync(List<Blogs> blogs);
        Task DeleteAllBlogsAsync();
        Task DeleteBlogAsync(int id);
        Task<List<Blogs>> GetBlogsAsync();
    }
}