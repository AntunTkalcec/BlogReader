using BlogReader.Models;
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
    public partial class ArticlesPage : ContentPage
    {
        public ArticlesPage(string articleLink = null)
        {
            InitializeComponent();
            BindingContext = viewModel = new ArticlesPageViewModel();
            if (articleLink != null)
            {
                viewModel.OpenBrowser(articleLink);
            }
        }
        private ArticlesPageViewModel viewModel;

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!viewModel.Initialized)
            {
                viewModel.Initialized = true;
            }
            else
            {
                viewModel.RefreshCollectionView();
            }
        }
    }
}