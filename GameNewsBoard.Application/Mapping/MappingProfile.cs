using System.Globalization;
using AutoMapper;
using GameNewsBoard.Application.DTOs;
using GameNewsBoard.Application.Responses.DTOs;
using GameNewsBoard.Application.Responses.DTOs.Responses;
using GameNewsBoard.Domain.Entities;

namespace GameNewsBoard.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GameResponse, Game>()
                .ForMember(dest => dest.Released, opt => opt.MapFrom(src => ParseDate(src.Released)))
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Game, GameDTO>()
                .ForMember(dest => dest.Released, opt => opt.MapFrom(src => src.Released.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)));

            CreateMap<Game, GameResponse>()
                .ForMember(dest => dest.Released, opt => opt.MapFrom(src => src.Released.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)));

            CreateMap<TierList, TierListResponse>();
            CreateMap<TierListEntry, TierListEntryResponse>();

            CreateMap<User, UserProfileResponse>()
               .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));

        }

        private static DateTimeOffset ParseDate(string released)
        {
            if (DateTimeOffset.TryParseExact(released, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            {
                return date;
            }

            // Data inválida → define uma default (ex: Unix epoch ou DateTimeOffset.MinValue)
            return DateTimeOffset.MinValue;
        }
    }

}