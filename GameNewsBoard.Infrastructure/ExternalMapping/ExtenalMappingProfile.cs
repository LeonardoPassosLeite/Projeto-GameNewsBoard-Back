using AutoMapper;
using GameNewsBoard.Application.Responses.DTOs;
using GameNewsBoard.Application.Responses.DTOs.Responses;
using GameNewsBoard.Domain.Entities;
using GameNewsBoard.Infrastructure.ExternalDtos;

namespace GameNewsBoard.Application.Mapping
{
    public class ExtenalMappingProfile : Profile
    {
        public ExtenalMappingProfile()
        {
            CreateMap<GameNewsDto, GameNewsArticleResponse>();

            CreateMap<GNewsResponseWrapper, GameNewsResponse>()
                .ForMember(dest => dest.Articles, opt => opt.MapFrom(src => src.Articles));

            CreateMap<IgdbGameDto, Game>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CoverImage, opt => opt.MapFrom(src =>
                    src.Cover != null && !string.IsNullOrEmpty(src.Cover.Url)
                        ? $"https:{src.Cover.Url}"
                        : string.Empty))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.AggregatedRating ?? src.UserRating ?? 0))
                .ForMember(dest => dest.Released, opt => opt.MapFrom(src =>
                    src.FirstReleaseDateUnix.HasValue
                        ? DateTimeOffset.FromUnixTimeSeconds(src.FirstReleaseDateUnix.Value)
                        : DateTimeOffset.MinValue))
                .ForMember(dest => dest.Platform, opt => opt.MapFrom(src =>
                    src.Platforms != null && src.Platforms.Any()
                        ? string.Join(", ", src.Platforms.Select(p => p.Name))
                        : "Unknown"))
                .ForMember(dest => dest.Id, opt => opt.Ignore());

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
                        : "Unavailable"))
                .ForMember(dest => dest.Platform, opt =>
                    opt.MapFrom(src =>
                        src.Platforms != null && src.Platforms.Any()
                            ? string.Join(", ", src.Platforms.Select(p => p.Name))
                            : "Unknown"))
                .ForMember(dest => dest.Id, opt => opt.Ignore());

        }
    }
}