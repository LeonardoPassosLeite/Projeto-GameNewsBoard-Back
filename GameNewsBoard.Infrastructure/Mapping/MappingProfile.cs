using AutoMapper;
using GameNewsBoard.Application.Responses.DTOs;
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

            CreateMap<IgdbGameDto, GameResponse>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CoverImage, opt =>
                    opt.MapFrom(src =>
                        src.Cover != null && !string.IsNullOrEmpty(src.Cover.Url)
                        ? $"https:{src.Cover.Url}"
                        : string.Empty
                    ))
                .ForMember(dest => dest.Rating, opt =>
                    opt.MapFrom(src => src.AggregatedRating ?? src.UserRating ?? 0))
                .ForMember(dest => dest.Released, opt => opt.MapFrom(src =>
                     src.FirstReleaseDateUnix.HasValue
                     ? DateTimeOffset.FromUnixTimeSeconds(src.FirstReleaseDateUnix.Value).ToString("dd/MM/yyyy")
                     : "Data não disponível"))
                .ForMember(dest => dest.Platform, opt =>
                    opt.MapFrom(src =>
                        src.Platforms != null && src.Platforms.Any()
                            ? string.Join(", ", src.Platforms.Select(p => p.Name))
                            : "Desconhecida"));
        }
    }
}