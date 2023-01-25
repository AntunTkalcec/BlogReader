using BlogReaderSharedLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogReader.Models
{
    public static class Extensions
    {
        public static Blogs FromDTO(this BlogDTO blog)
        {
            return new Blogs
            {
                Name = blog.Name,
                Description = blog.Description,
                RootLink = blog.RootLink,
                ImageUrl = blog.ImageUrl,
                SourceID = blog.SourceID,
                RealBlogID = blog.ID
            };
        }
        public static Articles FromDTOArticles(this ArticleDTO article)
        {
            return new Articles
            {
                Name = article.Name,
                RealArticleID = article.ID,
                ArticleID = article.ArticleID,
                Summary = article.Summary,
                RootLink = article.RootLink,
                ContentHTML = article.ContentHTML,
                ContentText = article.ContentText,
                Creator = article.Creator,
                PublishDate = (DateTime)article.PublishDate,
                Categories = article.Categories,
                BlogID = article.BlogID,
                ImageURL = article.ImageURL,
            };
        }
    }
}
