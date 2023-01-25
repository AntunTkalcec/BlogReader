using AutoMapper;
using BlogReaderCoreLibrary.Entities;
using BlogReaderCoreLibrary.Interfaces;
using BlogReaderSharedLibrary.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BlogReaderAPI.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBlogRepository _blogRepository;
        public BlogController(IMapper mapper, IBlogRepository blogRepo)
        {
            _mapper = mapper;
            _blogRepository = blogRepo;
        }
        
        //get all blogs in the database
        [HttpGet("blogs")]
        public async Task<List<BlogDTO>> GetBlogs()
        {
            List<Blog> blogs = await _blogRepository.GetBlogsAsync();
            List<BlogDTO> blogsDTO = _mapper.Map<List<BlogDTO>>(blogs);
            return blogsDTO;
        }

        //get all blogs from the given source
        [HttpGet("sources/{sourceID}/blogs")]
        public async Task<List<BlogDTO>> GetBlogsBySource(int sourceID)
        {
            List<Blog> blogs = await _blogRepository.GetBlogsFromSourceAsync(sourceID);
            List<BlogDTO> blogsDTO = _mapper.Map<List<BlogDTO>>(blogs);
            return blogsDTO;
        }
    }
}
