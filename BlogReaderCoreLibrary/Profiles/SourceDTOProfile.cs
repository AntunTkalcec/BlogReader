using AutoMapper;
using BlogReaderCoreLibrary.Entities;
using BlogReaderSharedLibrary.DTOs;

namespace BlogReaderCoreLibrary.Profiles
{
    public class SourceDTOProfile : Profile
    {
        public SourceDTOProfile()
        {
            CreateMap<Source, SourceDTO>();
        }
    }
}
