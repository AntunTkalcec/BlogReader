using BlogReaderSharedLibrary.DTOs;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using BlogReader.Services;
using System.ComponentModel;

namespace BlogReader.Models
{
    [Table("Articles")]
    public class Articles : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int ID { get; set; }
        public int RealArticleID { get; set; }
        public string ArticleID { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string RootLink { get; set; }
        public string ContentHTML { get; set; }
        public string ContentText  { get; set; }
        public string Creator { get; set; }
        public DateTime PublishDate { get; set; }
        public string Categories { get; set; }
        public int BlogID { get; set; }
        public string ImageURL { get; set; }
        public bool Favorite { get; set; }
        public bool ForOffline { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
