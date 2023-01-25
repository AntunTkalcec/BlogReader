using BlogReaderSharedLibrary.DTOs;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogReader.Models
{
    [Table("Blogs")]
    public class Blogs
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string RootLink { get; set; }
        public string ImageUrl { get; set; }
        public int SourceID { get; set; }
        public int RealBlogID { get; set; }
    }
}
