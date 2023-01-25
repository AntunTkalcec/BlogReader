using System.ComponentModel.DataAnnotations;

namespace BlogReaderCoreLibrary.Entities
{
    public class Blog
    {
        public int ID { get; set; }
        [MaxLength(50)]
        public string? Name { get; set; }
        [MaxLength(100)]
        public string? Description { get; set; }
        [MaxLength(150)]
        public string? RootLink { get; set; }
        [MaxLength(150)]
        public string? ImageUrl { get; set; }
        public int SourceID { get; set; }
        public string? Cron { get; set; }
        public Source? Source { get; set; }
        public List<Article>? Articles { get; set; }
    }
}
