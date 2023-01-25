using BlogReaderCoreLibrary.Entities;
using BlogReaderCoreLibrary.Interfaces;
using BlogReaderInfrastructureLibrary.Database;
using Microsoft.EntityFrameworkCore;

namespace BlogReaderInfrastructureLibrary.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly BlogReaderDBContext _db;
        public ArticleRepository(BlogReaderDBContext db)
        {
            _db = db;
        }
        public bool CreateArticle(Article article)
        {
            _db.Articles.Add(article);
            return Save();
        }
        public bool DeleteArticle(Article article)
        {
            _db.Articles.Remove(article);
            return Save();
        }
        public async Task<List<Article>> GetArticlesAsync()
        {
            return await _db.Articles.ToListAsync();
        }
        public bool Save()
        {
            return _db.SaveChanges() >= 0;
        }
        public bool UpdateArticle(Article article)
        {
            _db.Articles.Update(article);
            return Save();
        }
        public bool ArticleExists(int id)
        {
            return _db.Articles.Any(x => x.Id == id);
        }
        public async Task<Article> GetArticleAsync(int id)
        {
            return await _db.Articles.FirstOrDefaultAsync(x => x.Id == id);
        }
        public bool ArticleExists(string id)
        {
            bool value = _db.Articles.Any(y => y.ArticleID == id);
            return value;
        }

        public async Task<List<Article>> GetArticlesAsync(int id)
        {
            return await _db.Articles.Where(x => x.BlogID == id).ToListAsync();
        }

        public async Task<List<Article>> GetArticlesAsync(DateTime timeFrom, DateTime timeTo)
        {
            return await _db.Articles.Where(x => x.PublishDate >= timeFrom && x.PublishDate <= timeTo).ToListAsync();
        }

        public async Task<List<Article>> GetArticlesAsync(string category)
        {
            return await _db.Articles.Where(x => x.Categories.Contains(category)).ToListAsync();
        }

        public async Task<List<Article>> GetArticlesByTermAsync(string term)
        {
            return await _db.Articles.FromSqlRaw($"SELECT * FROM dbo.SearchContentByTerm('{term}');").ToListAsync();
        }

        public async Task<List<Article>> GetArticlesFromBlogsAsync(string blogs)
        {
            var blogArray = blogs.Split(",");
            return await _db.Articles.Where(x => blogArray.Contains(x.BlogID.ToString())).ToListAsync();
        }

        public async Task<List<Article>> GetArticlesFromBlogsAsync(string blogs, DateTime timeFrom, DateTime timeTo)
        {
            var blogArray = blogs.Split(",");
            return await _db.Articles.Where(x => blogArray.Contains(x.BlogID.ToString()) && x.PublishDate >= timeFrom && x.PublishDate <= timeTo).ToListAsync();
        }

        public async Task<List<Article>> GetArticlesFromSourceAsync(int sourceID)
        {
            return await _db.Articles.Include(a => a.Blog).Where(a => a.Blog.SourceID == sourceID).ToListAsync();
        }

        public Task<List<Article>> GetArticlesFromDBAsync(List<Article> articles)
        {
            return _db.Articles.Where(x => articles.Select(y => y.ArticleID).Contains(x.ArticleID)).ToListAsync();
        }
    }
}
