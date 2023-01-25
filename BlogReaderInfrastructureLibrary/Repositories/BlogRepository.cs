using BlogReaderCoreLibrary.Entities;
using BlogReaderCoreLibrary.Interfaces;
using BlogReaderInfrastructureLibrary.Database;
using Microsoft.EntityFrameworkCore;

namespace BlogReaderInfrastructureLibrary.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly BlogReaderDBContext _db;
        public BlogRepository(BlogReaderDBContext db)
        {
            _db = db;
        }
        public bool CreateBlog(Blog blog)
        {
            _db.Blogs.Add(blog);
            return Save();
        }
        public bool DeleteBlog(Blog blog)
        {
            _db.Blogs.Remove(blog);
            return Save();
        }
        public IQueryable<Blog> GetBlogs() 
        { 
            return _db.Blogs; 
        }
        public bool Save()
        {
            return _db.SaveChanges() >= 0;
        }
        public bool UpdateBlog(Blog blog)
        {
            _db.Blogs.Update(blog);
            return Save();
        }
        public bool BlogExists(int id)
        {
            return _db.Blogs.Any(x => x.ID == id);
        }
        public Task<Blog> GetBlogAsync(int id)
        {
            return _db.Blogs.FirstOrDefaultAsync(x => x.ID == id);
        }
        public bool BlogExists(string name)
        {
            bool value = _db.Blogs.Any(y => y.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public Task<List<Blog>> GetBlogsFromSourceAsync(int id)
        {
            return _db.Blogs.AsNoTracking().Where(x => x.SourceID == id).ToListAsync();
        }

        public Task<List<Blog>> GetBlogsAsync()
        {
            return _db.Blogs.AsNoTracking().ToListAsync();
        }
        public Task<List<Blog>> GetBlogsAndSourceAsync()
        {
            return _db.Blogs.Include(b => b.Source).Where(b => b.Source.ID == b.SourceID).ToListAsync();
        }
    }
}
