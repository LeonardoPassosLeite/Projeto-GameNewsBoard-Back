using System.Globalization;
using AutoMapper;
using GameNewsBoard.Application.Responses.DTOs;
using GameNewsBoard.Application.Responses.DTOs.Responses;
using GameNewsBoard.Domain.Entities;
using GameNewsBoard.Infrastructure.ExternalDtos;

namespace GameNewsBoard.Application.Mapping
{
    public class ExternalMappingProfile : Profile
    {
        public ExternalMappingProfile()
        {
            // Game News
            CreateMap<GameNewsDto, GameNewsArticleResponse>()
                        .ForMember(dest => dest.PubDate, opt => opt.MapFrom(src => src.PubDate.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture)));  

            CreateMap<GNewsResponseWrapper, GameNewsResponse>();

            // List All Games
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
                .ForMember(dest => dest.CoverImage, opt => opt.MapFrom(src =>
                    src.Cover != null && !string.IsNullOrEmpty(src.Cover.Url)
                        ? $"https:{src.Cover.Url}"
                        : string.Empty))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.AggregatedRating ?? src.UserRating ?? 0))
                .ForMember(dest => dest.Released, opt => opt.MapFrom(src =>
                    src.FirstReleaseDateUnix.HasValue
                        ? DateTimeOffset.FromUnixTimeSeconds(src.FirstReleaseDateUnix.Value).ToString("dd/MM/yyyy")
                        : "Unavailable"))
                .ForMember(dest => dest.Platform, opt => opt.MapFrom(src =>
                    src.Platforms != null && src.Platforms.Any()
                        ? string.Join(", ", src.Platforms.Select(p => p.Name))
                        : "Unknown"))
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // Releases
            CreateMap<IgdbGameReleaseWrapperDto, GameReleaseResponse>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Game.Name))
                .ForMember(dest => dest.Platform, opt => opt.MapFrom(src =>
                    src.Game.Platforms != null
                        ? string.Join(", ", src.Game.Platforms.Select(p => p.Name))
                        : string.Empty))
                .ForMember(dest => dest.CoverImage, opt => opt.MapFrom(src =>
                    src.Game.Cover != null
                        ? $"https:{src.Game.Cover.Url.Replace("t_thumb", "t_cover_big")}"
                        : string.Empty))
                .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src =>
                    src.Date.HasValue
                        ? DateTimeOffset.FromUnixTimeSeconds(src.Date.Value).ToString("yyyy-MM-dd")
                        : string.Empty));

        }
    }
}