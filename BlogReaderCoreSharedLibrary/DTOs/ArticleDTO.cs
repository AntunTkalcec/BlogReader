namespace BlogReaderSharedLibrary.DTOs
{
    public class ArticleDTO
    {
        public int ID { get; set; }
        public string? ArticleID { get; set; }
        public string? ImageURL { get; set; }
        public string? Name { get; set; }
        public string? Summary { get; set; }
        public string? RootLink { get; set; }
        public string? ContentHTML { get; set; }
        public string? ContentText { get; set; }
        public string? Creator { get; set; }
        public DateTime? PublishDate { get; set; }
        public string? Categories { get; set; }
        public int BlogID { get; set; }
        public BlogDTO? Blog { get; set; }
        public List<ArticleDTO>? ArticleList { get; set; }
        public int ArticleCount { get; set; }
        public string? ErrorCode { get; set; }
    }
}
