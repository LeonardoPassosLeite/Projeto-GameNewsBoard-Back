using System.Globalization;
using AutoMapper;
using GameNewsBoard.Application.DTOs;
using GameNewsBoard.Application.Responses.DTOs;
using GameNewsBoard.Domain.Entities;

namespace GameNewsBoard.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Game, GameDTO>()
                .ForMember(dest => dest.Released, opt => opt.MapFrom(src => src.Released.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)));
            CreateMap<Game, GameResponse>()
                .ForMember(dest => dest.Released, opt => opt.MapFrom(src => src.Released.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)));
        }
    }
}