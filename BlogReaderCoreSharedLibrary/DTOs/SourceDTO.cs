namespace BlogReaderSharedLibrary.DTOs
{
    public class SourceDTO
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? RootLink { get; set; }
        public List<BlogDTO>? Blogs { get; set; }
        public List<SourceDTO>? SourceList { get; set; }
        public int SourceCount { get; set; }
        public string? ErrorCode { get; set; }
    }
}
