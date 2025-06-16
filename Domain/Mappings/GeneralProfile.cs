using AutoMapper;
using DTO.UserDTO;
using DTO.DirectorateDTO;
using Entities.Models;
using Helpers.Enumerations;
using System;
using DTO.VehicleDTO;
using DTO.VehicleRequest;
using DTO.FineDTO;

namespace Domain.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            #region UserDTO <-> Auto_Users  

            CreateMap<Auto_Users, UserDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.DirectorateName, opt => opt.MapFrom(src => src.Directorate != null ? src.Directorate.DirectoryName : null));

            CreateMap<UserDTO, Auto_Users>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<UserStatus>(src.Status)))
                .ForMember(dest => dest.IsSpecialist, opt => opt.MapFrom(src => src.Role == "Specialist"))
                .ForMember(dest => dest.IDFK_Directory, opt => opt.Ignore())
                .ForMember(dest => dest.Directorate, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.NormalizedEmail, opt => opt.Ignore())
                .ForMember(dest => dest.NormalizedUserName, opt => opt.Ignore())
                .ForMember(dest => dest.EmailConfirmed, opt => opt.Ignore())
                .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.Ignore())
                .ForMember(dest => dest.TwoFactorEnabled, opt => opt.Ignore())
                .ForMember(dest => dest.LockoutEnabled, opt => opt.Ignore())
                .ForMember(dest => dest.AccessFailedCount, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedIp, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedIp, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedOn, opt => opt.Ignore());

            #endregion

            #region RegisterDTO <-> Auto_Users  

            CreateMap<RegisterDTO, Auto_Users>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.FatherName, opt => opt.MapFrom(src => src.FatherName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
                .ForMember(dest => dest.IsSpecialist, opt => opt.MapFrom(src => src.Role == "Specialist"))
                .ForMember(dest => dest.SpecialistNumber, opt => opt.MapFrom(src => src.SpecialistNumber))
                .ForMember(dest => dest.IDFK_Directory, opt => opt.MapFrom(src => src.DirectorateId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => UserStatus.Pending))
                .ForMember(dest => dest.NormalizedUserName, opt => opt.Ignore())
                .ForMember(dest => dest.NormalizedEmail, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.EmailConfirmed, opt => opt.Ignore())
                .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.Ignore())
                .ForMember(dest => dest.TwoFactorEnabled, opt => opt.Ignore())
                .ForMember(dest => dest.LockoutEnabled, opt => opt.Ignore())
                .ForMember(dest => dest.AccessFailedCount, opt => opt.Ignore())
                .ForMember(dest => dest.Directorate, opt => opt.Ignore());

            CreateMap<Auto_Users, RegisterDTO>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.IsSpecialist ? "Specialist" : "Individ"))
                .ForMember(dest => dest.SpecialistNumber, opt => opt.MapFrom(src => src.SpecialistNumber))
                .ForMember(dest => dest.DirectorateId, opt => opt.MapFrom(src => src.IDFK_Directory))
                .ForMember(dest => dest.Password, opt => opt.Ignore());

            #endregion

            #region UpdateUserDTO -> Auto_Users  

            CreateMap<UpdateUserDTO, Auto_Users>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
                .ForMember(dest => dest.ModifiedIp, opt => opt.MapFrom(src => src.ModifiedIp))
                .ForMember(dest => dest.ModifiedOn, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.FirstName, opt => opt.Ignore())
                .ForMember(dest => dest.FatherName, opt => opt.Ignore())
                .ForMember(dest => dest.LastName, opt => opt.Ignore())
                .ForMember(dest => dest.BirthDate, opt => opt.Ignore())
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.IsSpecialist, opt => opt.Ignore())
                .ForMember(dest => dest.IDFK_Directory, opt => opt.Ignore())
                .ForMember(dest => dest.SpecialistNumber, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedIp, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.Ignore())
                .ForMember(dest => dest.EmailConfirmed, opt => opt.Ignore())
                .ForMember(dest => dest.NormalizedEmail, opt => opt.Ignore())
                .ForMember(dest => dest.NormalizedUserName, opt => opt.Ignore())
                .ForMember(dest => dest.AccessFailedCount, opt => opt.Ignore())
                .ForMember(dest => dest.LockoutEnabled, opt => opt.Ignore())
                .ForMember(dest => dest.TwoFactorEnabled, opt => opt.Ignore())
                .ForMember(dest => dest.Directorate, opt => opt.Ignore());

            #endregion

            #region LoginDTO <-> Auto_Users  

            CreateMap<Auto_Users, LoginDTO>().ReverseMap();

            #endregion

            #region Directorate  

            CreateMap<Auto_Directorates, DirectorateDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IDPK_Directory))
                .ReverseMap()
                .ForMember(dest => dest.IDPK_Directory, opt => opt.MapFrom(src => src.Id));

            #endregion

            #region Vehicle  

            CreateMap<Auto_Vehicles, VehicleDTO>().ReverseMap();
            CreateMap<Auto_Vehicles, VehicleRegisterDTO>().ReverseMap();

            #endregion

            #region VehicleRequest
            CreateMap<Auto_VehicleChangeRequests, VehicleRequestDTO>().ReverseMap();
            CreateMap<Auto_VehicleChangeRequests, VehicleChangeStatusDTO>().ReverseMap();
            #endregion

            CreateMap<Auto_VehicleChangeRequests, VehicleRequestListDTO>()
    .ForMember(dest => dest.PlateNumber, opt => opt.MapFrom(src => src.Vehicle.PlateNumber))
    .ReverseMap();

            CreateMap<Auto_Fines, FineDTO>().ReverseMap();
            CreateMap<Auto_FineRecipients, FineRecipientDTO>().ReverseMap();

            CreateMap<Auto_Fines, FineResponseDTO>()
    .ForMember(dest => dest.PoliceFullName,
        opt => opt.MapFrom(src => src.PoliceOfficer.FirstName + " " + src.PoliceOfficer.LastName))
    .ForMember(dest => dest.RecipientFullName,
        opt => opt.MapFrom(src => src.FineRecipient.FirstName + " " + src.FineRecipient.LastName))
    .ForMember(dest => dest.PlateNumber,
        opt => opt.MapFrom(src => src.FineRecipient.PlateNumber))
    .ReverseMap();



        }
    }
}
