using AutoMapper;
using DTO.UserDTO;
using Entities.Models;

namespace Domain.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            // RegisterDto → Auto_Users
            CreateMap<RegisterDto, Auto_Users>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.IsSpecialist, opt => opt.MapFrom(_ => false))
                .ForMember(dest => dest.IsApproved, opt => opt.MapFrom(_ => false))
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(_ => "system"))
                .ForMember(dest => dest.Invalidated, opt => opt.MapFrom(_ => (byte)0))
                .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.Ignore())
                .ForMember(dest => dest.EmailConfirmed, opt => opt.Ignore())
                .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
                .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());

            // LoginDto → Auto_Users (nuk përdoret realisht për mapping, por po e lëmë për uniformitet)
            CreateMap<LoginDto, Auto_Users>();
        }
    }
}
