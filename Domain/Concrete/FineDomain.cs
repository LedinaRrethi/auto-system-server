using AutoMapper;
using DAL.Concrete;
using DAL.Contracts;
using DAL.UoW;
using Domain.Contracts;
using DTO.FineDTO;
using DTO.VehicleDTO;
using Entities.Models;
using Helpers.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public class FineDomain : IFineDomain
    {
        private readonly IFineRepository _repo;
        private readonly IMapper _mapper;
        private readonly UserManager<Auto_Users> _userManager;
        private readonly IUnitOfWork _unitOfWork;


        public FineDomain(IFineRepository repo, IMapper mapper, UserManager<Auto_Users> userManager, IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _mapper = mapper;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }



        public async Task<bool> CreateFineAsync(FineCreateDTO dto, string policeId, string ip)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var vehicle = await _repo.GetVehicleByPlateAsync(dto.PlateNumber);
                Auto_Users? owner = vehicle?.IDFK_Owner != null
                    ? await _repo.GetUserByIdAsync(vehicle.IDFK_Owner)
                    : null;

                Auto_FineRecipients? recipient = owner != null
                    ? await _repo.GetFineRecipientByUserIdAsync(owner.Id)
                    : await _repo.GetFineRecipientByPersonalIdAsync(dto.PersonalId!);

                if (recipient == null)
                {
                    recipient = new Auto_FineRecipients
                    {
                        IDPK_FineRecipient = Guid.NewGuid(),
                        IDFK_User = owner?.Id,
                        FirstName = owner?.FirstName ?? dto.FirstName!,
                        LastName = owner?.LastName ?? dto.LastName!,
                        FatherName = owner?.FatherName ?? dto.FatherName,
                        PersonalId = owner?.PersonalId ?? dto.PersonalId,
                        PlateNumber = dto.PlateNumber,
                        CreatedBy = policeId,
                        CreatedOn = DateTime.UtcNow,
                        CreatedIp = ip
                    };

                    await _repo.AddFineRecipientAsync(recipient);
                    await _repo.SaveChangesAsync();
                }

                var fine = new Auto_Fines
                {
                    IDPK_Fine = Guid.NewGuid(),
                    IDFK_Vehicle = vehicle?.IDPK_Vehicle,
                    IDFK_FineRecipient = recipient.IDPK_FineRecipient,
                    FineAmount = dto.FineAmount,
                    FineDate = dto.FineDate ?? DateTime.UtcNow,
                    FineReason = dto.FineReason,
                    CreatedBy = policeId,
                    CreatedOn = DateTime.UtcNow,
                    CreatedIp = ip
                };

                await _repo.AddFineAsync(fine);
                await _repo.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task<PaginationResult<FineResponseDTO>> GetMyFinesAsync(string userId, FineFilterDTO filter, int page, int pageSize)
        {
            var items = await _repo.GetFinesForUserAsync(userId, filter, page, pageSize + 1);
            var result = _mapper.Map<List<FineResponseDTO>>(items);

            return new PaginationResult<FineResponseDTO>
            {
                Items = result.Take(pageSize).ToList(),
                Page = page,
                PageSize = pageSize,
                HasNextPage = result.Count > pageSize
            };
        }

        public async Task<PaginationResult<FineResponseDTO>> SearchFinesByPlateAsync(string plate, int page, int pageSize)
        {
            var items = await _repo.SearchFinesByPlateAsync(plate, page, pageSize + 1);
            var result = _mapper.Map<List<FineResponseDTO>>(items);

            return new PaginationResult<FineResponseDTO>
            {
                Items = result.Take(pageSize).ToList(),
                Page = page,
                PageSize = pageSize,
                HasNextPage = result.Count > pageSize
            };
        }


        public async Task<List<FineResponseDTO>> GetAllFinesAsync(int page, int pageSize)
        {
            var fines = await _repo.GetAllFinesAsync(page, pageSize);
            return _mapper.Map<List<FineResponseDTO>>(fines);
        }

     

        public async Task<PaginationResult<FineResponseDTO>> GetFinesCreatedByPoliceAsync(string policeId, FineFilterDTO filter, int page, int pageSize)
        {
            var items = await _repo.GetFinesCreatedByPoliceAsync(policeId, filter, page, pageSize + 1);
            var result = _mapper.Map<List<FineResponseDTO>>(items);

            return new PaginationResult<FineResponseDTO>
            {
                Items = result.Take(pageSize).ToList(),
                Page = page,
                PageSize = pageSize,
                HasNextPage = result.Count > pageSize
            };
        }


        public async Task<object?> GetRecipientDetailsByPlateAsync(string plate)
        {
            var vehicle = await _repo.GetVehicleByPlateAsync(plate);

            if (vehicle != null && vehicle.IDFK_Owner != null)
            {
                var owner = await _repo.GetUserByIdAsync(vehicle.IDFK_Owner);
                if (owner != null)
                {
                    return new
                    {
                        IsFrom = "Vehicle",
                        FirstName = owner.FirstName,
                        FatherName = owner.FatherName,
                        LastName = owner.LastName,
                        PersonalId = owner.PersonalId
                    };
                }
            }

            var recipient = await _repo.GetFineRecipientByPlateAsync(plate);
            if (recipient != null)
            {
                return new
                {
                    IsFrom = "FineRecipient",
                    FirstName = recipient.FirstName,
                    FatherName = recipient.FatherName,
                    LastName = recipient.LastName,
                    PersonalId = recipient.PersonalId
                };
            }

            return null;
        }




    }
}
