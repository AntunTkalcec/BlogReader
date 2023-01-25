namespace BlogReaderSharedLibrary.DTOs
{
    public class BlogDTO
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? RootLink { get; set; }
        public string? ImageUrl { get; set; }
        public int SourceID { get; set; }
        public string? Cron { get; set; }
        public SourceDTO? Source { get; set; }
        public List<ArticleDTO>? Articles { get; set; }
        public List<BlogDTO>? BlogList { get; set; }
        public int BlogCount { get; set; }
        public string? ErrorCode { get; set; }
    }
}
