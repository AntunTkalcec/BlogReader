using AutoMapper;
using BlogReaderCoreLibrary.Entities;
using BlogReaderCoreLibrary.Interfaces;
using BlogReaderSharedLibrary.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BlogReaderAPI.Controllers
{
    [Route("api/v1/sources")]
    [ApiController]
    public class SourceController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISourceRepository _sourceRepository;
        public SourceController(IMapper mapper, ISourceRepository sourceRepo)
        {
            _mapper = mapper;
            _sourceRepository = sourceRepo;
        }
        [HttpGet]
        public async Task<List<SourceDTO>> GetSourcesAsync()
        {
            List<Source> sources = await _sourceRepository.GetSourcesAsync();
            List<SourceDTO> sourcesDTO = _mapper.Map<List<SourceDTO>>(sources);
            return sourcesDTO;
        }
    }
}
