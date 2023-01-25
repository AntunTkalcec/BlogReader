using System.ComponentModel.DataAnnotations;

namespace BlogReaderCoreLibrary.Entities
{
    public class Source
    {
        public int ID { get; set; }
        [MaxLength(50)]
        public string? Name { get; set; }
        [MaxLength(150)]
        public string? RootLink { get; set; }
        public List<Blog>? Blogs { get; set; }
    }
}
