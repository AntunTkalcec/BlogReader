using BlogReaderCoreLibrary.Entities;

namespace BlogReaderCoreLibrary.Interfaces
{
    public interface ISourceRepository
    {
        Task<List<Source>> GetSourcesAsync();
        Task<Source> GetSourceAsync(int id);
        bool SourceExists(int id);
        bool SourceExists(string name);
        bool CreateSource(Source source);
        bool UpdateSource(Source source);
        bool DeleteSource(Source source);
        bool Save();
    }
}
