using BlogReaderCoreLibrary.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogReaderInfrastructureLibrary.Database
{
    public class BlogReaderDBContext : DbContext
    {
        public BlogReaderDBContext(DbContextOptions<BlogReaderDBContext> options)
            : base(options)
        {

        }
        public DbSet<Source> Sources => Set<Source>();
        public DbSet<Blog> Blogs => Set<Blog>();
        public DbSet<Article> Articles => Set<Article>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Article>().HasKey(a => a.Id);
            modelBuilder.Entity<Article>().HasIndex(a => a.ArticleID);
        }
    }
}
