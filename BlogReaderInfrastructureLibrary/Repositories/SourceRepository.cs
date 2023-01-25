using BlogReaderCoreLibrary.Entities;
using BlogReaderCoreLibrary.Interfaces;
using BlogReaderInfrastructureLibrary.Database;
using Microsoft.EntityFrameworkCore;

namespace BlogReaderInfrastructureLibrary.Repositories
{
    public  class SourceRepository :ISourceRepository
    {
        private readonly BlogReaderDBContext _db;
        public SourceRepository(BlogReaderDBContext db)
        {
            _db = db;
        }
        public bool CreateSource(Source source)
        {
            _db.Sources.Add(source);
            return Save();
        }
        public bool DeleteSource(Source source)
        {
            _db.Sources.Remove(source);
            return Save();
        }
        public async Task<List<Source>> GetSourcesAsync()
        {
            return await _db.Sources.ToListAsync();
        }
        public bool Save()
        {
            return _db.SaveChanges() >= 0;
        }
        public bool UpdateSource(Source source)
        {
            _db.Sources.Update(source);
            return Save();
        }
        public bool SourceExists(int id)
        {
            return _db.Sources.Any(x => x.ID == id);
        }
        public async Task<Source> GetSourceAsync(int id)
        {
            return await _db.Sources.FirstOrDefaultAsync(x => x.ID == id);
        }
        public bool SourceExists(string name)
        {
            bool value = _db.Sources.Any(y => y.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }
    }
}
