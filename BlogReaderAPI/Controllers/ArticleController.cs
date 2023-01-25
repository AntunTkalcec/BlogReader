using AutoMapper;
using BlogReaderCoreLibrary.Entities;
using BlogReaderCoreLibrary.Interfaces;
using BlogReaderSharedLibrary.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BlogReaderAPI.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IArticleRepository _articleRepository;
        public ArticleController(IMapper mapper, IArticleRepository articleRepo)
        {
            _mapper = mapper;
            _articleRepository = articleRepo;
        }

        //get all articles from the database
        [HttpGet("articles")]
        public async Task<List<ArticleDTO>> GetArticles()
        {
            List<Article> articles = await _articleRepository.GetArticlesAsync();
            List<ArticleDTO> articlesDTO = _mapper.Map<List<ArticleDTO>>(articles);
            return articlesDTO;
        }
        //get all articles from the given blog
        [HttpGet("blog/{blogID}/articles")]
        public async Task<List<ArticleDTO>> GetArticlesFromBlog(int blogID)
        {
            List<Article> articles = await _articleRepository.GetArticlesAsync(blogID);
            List<ArticleDTO> articlesDTO = _mapper.Map<List<ArticleDTO>>(articles);
            return articlesDTO;
        }
        
        //get all articles from the given timeframe
        [HttpGet("articles/{timeFrom},{timeTo}")]
        public async Task<List<ArticleDTO>> GetArticlesFromTimeframe(DateTime timeFrom, DateTime timeTo)
        {
            List<Article> articles = await _articleRepository.GetArticlesAsync(timeFrom, timeTo);
            List<ArticleDTO> articlesDTO = _mapper.Map<List<ArticleDTO>>(articles);
            return articlesDTO;
        }

        //get all articles that belong to given category
        [HttpGet("category/{category}/articles")]
        public async Task<List<ArticleDTO>> GetArticlesFromCategory(string category)
        {
            List<Article> articles = await _articleRepository.GetArticlesAsync(category);
            List<ArticleDTO> articlesDTO = _mapper.Map<List<ArticleDTO>>(articles);
            return articlesDTO;
        }

        //get all articles that contain a given term
        [HttpGet("term/{term}/articles")]
        public async Task<List<ArticleDTO>> GetArticlesByTerm(string term)
        {
            List<Article> articles = await _articleRepository.GetArticlesByTermAsync(term);
            List<ArticleDTO> articlesDTO = _mapper.Map<List<ArticleDTO>>(articles);
            return articlesDTO;
        }
        
        //get all articles from the given list of blogs
        [HttpGet("blogs/{blogs}/articles")]
        public async Task<List<ArticleDTO>> GetArticlesFromBlogs(string blogs)
        {
            List<Article> articles = await _articleRepository.GetArticlesFromBlogsAsync(blogs);
            List<ArticleDTO> articlesDTO = _mapper.Map<List<ArticleDTO>>(articles);
            return articlesDTO;
        }

        //get all articles from the given list of blogs from the given timeframe
        [HttpGet("blogs/{blogs}/{timeFrom},{timeTo}/articles")]
        public async Task<List<ArticleDTO>> GetArticlesFromBlogsByTimeframe(string blogs, DateTime timeFrom, DateTime timeTo)
        {
            List<Article> articles = await _articleRepository.GetArticlesFromBlogsAsync(blogs, timeFrom, timeTo);
            List<ArticleDTO> articlesDTO = _mapper.Map<List<ArticleDTO>>(articles);
            return articlesDTO;
        }

        //get all articles from given source
        [HttpGet("sources/{sourceID}/articles")]
        public async Task<List<ArticleDTO>> GetArticlesFromSource(int sourceID)
        {
            List<Article> articles = await _articleRepository.GetArticlesFromSourceAsync(sourceID);
            List<ArticleDTO> articlesDTO = _mapper.Map<List<ArticleDTO>>(articles);
            return articlesDTO;
        }
    }
}
