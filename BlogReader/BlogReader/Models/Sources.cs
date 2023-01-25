using BlogReaderSharedLibrary.DTOs;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogReader.Models
{
    [Table("Sources")]
    public class Sources
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int ID { get; set; }
        public string Name { get; set; }
        public string RootLink { get; set; }
        public int RealSourceID { get; set; }
    }
}
