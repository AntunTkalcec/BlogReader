using BlogReaderCoreLibrary.Entities;
using BlogReaderCoreLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlogReaderAPI.Controllers
{
    [Route("api/v1/reader")]
    [ApiController]
    public class RssController : ControllerBase
    {
        private readonly IRSSreader _reader;
        public RssController(IRSSreader reader)
        {
            _reader = reader;
        }
        [HttpGet("{blogID}/articles")]
        public async Task<ActionResult<List<Article>>> GetArticles(int blogID)
        {
            List<Article> newArticles = await _reader.GetArticlesAndSaveToDb(blogID);
            if (newArticles.Count == 0) 
            {
                ModelState.AddModelError("", "There were no articles to add or something went wrong with creating an article.");
                return StatusCode(500, ModelState);
            }
            return Ok(newArticles.Count);
        }
    }
}
