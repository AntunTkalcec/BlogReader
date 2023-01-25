using BlogReader.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BlogReader.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SetupPage : ContentPage
    {
        public SetupPage()
        {
            InitializeComponent();
#if DEBUG
            DeleteBlogsButtonDEBUG.IsEnabled = true;
            DeleteBlogsButtonDEBUG.IsVisible = true;
#else
            DeleteBlogsButtonDEBUG.IsEnabled = false;
            DeleteBlogsButtonDEBUG.IsVisible = false;
#endif
        }
    }
}