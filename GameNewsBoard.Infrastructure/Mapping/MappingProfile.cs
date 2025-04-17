using AutoMapper;
using GameNewsBoard.Application.DTOs;
using GameNewsBoard.Infrastructure.ExternalDtos;

namespace GameNewsBoard.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GameNewsDto, GameNewsArticleResponse>();
            CreateMap<GNewsResponseWrapper, GameNewsResponse>()
                .ForMember(dest => dest.Articles, opt => opt.MapFrom(src => src.Articles));
        }
    }
}
