using AutoMapper;
using BlogReaderCoreLibrary.Entities;
using BlogReaderSharedLibrary.DTOs;

namespace BlogReaderCoreLibrary.Profiles
{
    public class ArticleDTOProfile : Profile
    {
        public ArticleDTOProfile()
        {
            CreateMap<Article, ArticleDTO>();
        }
    }
}
