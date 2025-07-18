﻿using AutoMapper;
using DTO.UserDTO;
using DTO.DirectorateDTO;
using Entities.Models;
using Helpers.Enumerations;
using System;
using DTO.VehicleDTO;
using DTO.VehicleRequest;
using DTO.FineDTO;
using DTO.InspectionDTO;

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
                .ForMember(dest => dest.IsSpecialist, opt => opt.MapFrom(src => src.Role == UserRole.Specialist))
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
                .ForMember(dest => dest.Directorate, opt => opt.Ignore())
.ForMember(dest => dest.PersonalId, opt => opt.MapFrom(src => src.PersonalId));


            CreateMap<Auto_Users, RegisterDTO>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.IsSpecialist ? "Specialist" : "Individ"))
                .ForMember(dest => dest.SpecialistNumber, opt => opt.MapFrom(src => src.SpecialistNumber))
                .ForMember(dest => dest.DirectorateId, opt => opt.MapFrom(src => src.IDFK_Directory))
                .ForMember(dest => dest.Password, opt => opt.Ignore())        
.ForMember(dest => dest.PersonalId, opt => opt.MapFrom(src => src.PersonalId));


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
            CreateMap<Auto_Vehicles, VehicleEditDTO>().ReverseMap();

            #endregion

            #region VehicleRequest
            CreateMap<Auto_VehicleChangeRequests, VehicleChangeStatusDTO>().ReverseMap();
            #endregion

            CreateMap<Auto_VehicleChangeRequests, VehicleRequestListDTO>()
    .ForMember(dest => dest.PlateNumber, opt => opt.MapFrom(src => src.Vehicle.PlateNumber))
    .ForMember(dest => dest.CurrentDataSnapshotJson, opt => opt.MapFrom(src => src.CurrentDataSnapshotJson))
    .ForMember(dest => dest.RequestDataJson, opt => opt.MapFrom(src => src.RequestDataJson))
    .ForMember(dest => dest.RequestType, opt => opt.MapFrom(src => src.RequestType))
    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
    .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn))
    .ForMember(dest => dest.IDPK_ChangeRequest, opt => opt.MapFrom(src => src.IDPK_ChangeRequest))
    .ForMember(dest => dest.IDFK_Vehicle, opt => opt.MapFrom(src => src.IDFK_Vehicle));


            CreateMap<Auto_Fines, FineResponseDTO>()
     .ForMember(dest => dest.PoliceFullName,
         opt => opt.MapFrom(src => src.PoliceOfficer.FirstName + " " + src.PoliceOfficer.LastName))
     .ForMember(dest => dest.RecipientFullName,
         opt => opt.MapFrom(src => src.FineRecipient.FirstName + " " + src.FineRecipient.LastName))
     .ForMember(dest => dest.PlateNumber,
         opt => opt.MapFrom(src =>
             src.Vehicle != null && !string.IsNullOrEmpty(src.Vehicle.PlateNumber)
                 ? src.Vehicle.PlateNumber
                 : src.FineRecipient.PlateNumber
         ))
     .ReverseMap();







            CreateMap<Auto_InspectionRequests, InspectionRequestCreateDTO>().ReverseMap();

            CreateMap<Auto_InspectionDocs, InspectionDocumentDTO>().ReverseMap();

        }
    }
}
