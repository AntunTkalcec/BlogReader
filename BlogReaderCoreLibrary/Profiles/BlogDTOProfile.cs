using AutoMapper;
using BlogReaderCoreLibrary.Entities;
using BlogReaderSharedLibrary.DTOs;

namespace BlogReaderCoreLibrary.Profiles
{
    public class BlogDTOProfile : Profile
    {
        public BlogDTOProfile()
        {
            CreateMap<Blog, BlogDTO>();
        }
    }
}
