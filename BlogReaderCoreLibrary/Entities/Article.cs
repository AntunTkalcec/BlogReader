using System.ComponentModel.DataAnnotations;

namespace BlogReaderCoreLibrary.Entities
{
    public class Article
    {
        public int Id { get; set; }
        public string? ArticleID { get; set; }
        [MaxLength(150)]
        public string? Name { get; set; }
        [MaxLength(350)]
        public string? ImageURL { get; set; }
        [MaxLength(1000)]
        public string? Summary { get; set; }
        [MaxLength(150)]
        public string? RootLink { get; set; }
        [MaxLength(7500)]
        public string? ContentHTML { get; set; }
        [MaxLength(7500)]
        public string? ContentText { get; set; }
        [MaxLength(25)]
        public string? Creator { get; set; }
        public DateTime? PublishDate { get; set; }
        [MaxLength(300)]
        public string? Categories { get; set; }
        public int BlogID { get; set; }
        public Blog? Blog { get; set; }
    }
}
