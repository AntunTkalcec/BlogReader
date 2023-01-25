using System.Text;
using BlogReaderCoreLibrary.Entities;
using BlogReaderCoreLibrary.Interfaces;
using BlogReaderInfrastructureLibrary.Database;
using CodeHollow.FeedReader;
using Microsoft.EntityFrameworkCore;
using HtmlAgilityPack;
using CodeHollow.FeedReader.Feeds;
using System.Security.Cryptography;

namespace BlogReaderInfrastructureLibrary.Services
{
    public class RSSreader : IRSSreader
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IBlogRepository _blogRepository;
        public RSSreader(IArticleRepository articleRepository, IBlogRepository blogRepository)
        {
            _articleRepository = articleRepository;
            _blogRepository = blogRepository;
        }
       
        public async Task<List<Article>> GetArticlesAsync(Blog blog)
        {
            try
            {
                List<Article> articles = new();

                var feed = await FeedReader.ReadAsync($"{blog.RootLink}/feed");
                foreach (var item in feed.Items)
                {
                    Article article = new()
                    {
                        ArticleID = ParseID(item.SpecificItem),
                        Name = item.Title,
                        ImageURL = ParseIMG(item.Content),
                        Summary = ParseDescription(item.Description),
                        RootLink = item.Link,
                        ContentHTML = item.Content,
                        ContentText = ParseContent(item.Content),
                        Creator = ParseAuthor(item.SpecificItem),
                        PublishDate = item.PublishingDate,
                        Categories = string.Join(", ", item.Categories),
                        BlogID = blog.ID,
                        Blog = blog
                    };

                    articles.Add(article);
                }
                return articles;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<List<Article>> GetArticlesFromDBAsync(List<Article> articles)
        {
            return _articleRepository.GetArticlesFromDBAsync(articles);
        }

        public Task<Blog> GetBlogAsync(int blogID)
        {
            return _blogRepository.GetBlogAsync(blogID);
        }

        private static string ParseIMG(string content)
        {
            List<string> listOfImageUrls = new();
            if (content.Contains("<img"))
            {
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(content);
                var imageUrl = htmlDoc.DocumentNode.SelectSingleNode("//img").GetAttributeValue("src", "");
                listOfImageUrls.Add(imageUrl);
                return imageUrl;
            }
            return "";
        } 

        private static string ParseID(BaseFeedItem specificItem)
        {
            foreach (var it in specificItem.Element.Elements())
            {
                if (it.Name.ToString().Contains("guid"))
                {
                    string ID = it.Value.Substring(it.Value.IndexOf("=") + 1) + " " + specificItem.Link;
                    string hash = CreateMD5Hash(ID);

                    return hash;
                }
            }
            return "ERROR";
        }

        private static string CreateMD5Hash(string iD)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(iD);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i< hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }

        private static string ParseAuthor(BaseFeedItem specificItem)
        {
            foreach (var it in specificItem.Element.Elements())
            {
                if (it.Name.ToString().Contains("creator"))
                {
                    return it.Value.Trim();
                }
            }
            return "Could not retrieve author.";
        }

        private static string ParseDescription(string description)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(description);

            return htmlDoc.DocumentNode.InnerText;
        }

        private static string ParseContent(string content)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(content);

            return htmlDoc.DocumentNode.InnerText;
        }

        public async Task<List<Article>> GetArticlesAndSaveToDb(int blogId)
        {
            Blog blog = await GetBlogAsync(blogId);
            List<Article> articles = await GetArticlesAsync(blog);
            List<Article> articlesFromDB = await GetArticlesFromDBAsync(articles);
            List<Article> newArticles = articles.ExceptBy(articlesFromDB.Select(x => x.ArticleID), y => y.ArticleID).ToList();

            return newArticles;
        }
    }
}
