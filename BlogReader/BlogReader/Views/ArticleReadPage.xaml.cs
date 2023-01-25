using BlogReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BlogReader.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ArticleReadPage : ContentPage
    {
        public ArticleReadPage(Articles article)
        {
            InitializeComponent();
            BindingContext = article;
        }
    }
}