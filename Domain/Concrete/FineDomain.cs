using AutoMapper;
using DAL.Contracts;
using DAL.UoW;
using Domain.Contracts;
using DTO;
using DTO.FineDTO;
using Entities.Models;
using Helpers.Pagination;
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



        //public async Task<bool> CreateFineAsync(FineCreateDTO dto, string policeId, string ip)
        //{
        //    using var transaction = await _unitOfWork.BeginTransactionAsync();

        //    try
        //    {
        //        var vehicle = await _repo.GetVehicleByPlateAsync(dto.PlateNumber);
        //        Auto_Users? owner = vehicle?.IDFK_Owner != null
        //            ? await _repo.GetUserByIdAsync(vehicle.IDFK_Owner)
        //            : null;

        //        Auto_FineRecipients? recipient = owner != null
        //            ? await _repo.GetFineRecipientByUserIdAsync(owner.Id)
        //            : await _repo.GetFineRecipientByPersonalIdAsync(dto.PersonalId!);

        //        if (recipient == null)
        //        {
        //            recipient = new Auto_FineRecipients
        //            {
        //                IDPK_FineRecipient = Guid.NewGuid(),
        //                IDFK_User = owner?.Id,
        //                FirstName = owner?.FirstName ?? dto.FirstName!,
        //                LastName = owner?.LastName ?? dto.LastName!,
        //                FatherName = owner?.FatherName ?? dto.FatherName,
        //                PersonalId = owner?.PersonalId ?? dto.PersonalId,
        //                PlateNumber = dto.PlateNumber,
        //                CreatedBy = policeId,
        //                CreatedOn = DateTime.UtcNow,
        //                CreatedIp = ip
        //            };

        //            await _repo.AddFineRecipientAsync(recipient);

        //        }

        //        var fine = new Auto_Fines
        //        {
        //            IDPK_Fine = Guid.NewGuid(),
        //            IDFK_Vehicle = vehicle?.IDPK_Vehicle,
        //            IDFK_FineRecipient = recipient.IDPK_FineRecipient,
        //            FineAmount = dto.FineAmount,
        //            FineDate = dto.FineDate ?? DateTime.UtcNow,
        //            FineReason = dto.FineReason,
        //            CreatedBy = policeId,
        //            CreatedOn = DateTime.UtcNow,
        //            CreatedIp = ip
        //        };

        //        await _repo.AddFineAsync(fine);

        //        await _unitOfWork.CommitAsync();
        //        await transaction.CommitAsync();
        //        return true;
        //    }
        //    catch
        //    {
        //        await transaction.RollbackAsync();
        //        throw;
        //    }
        //}

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
                        PersonalId = owner?.PersonalId ?? dto.PersonalId!,
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
                    IDFK_Vehicle = vehicle.IDPK_Vehicle,
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


        public async Task<PaginationResult<FineResponseDTO>> GetMyFinesAsync(string userId, FineFilterDTO filter)
        {
            var fines = await _repo.GetFinesForUserAsync(userId);
            var dtoList = _mapper.Map<List<FineResponseDTO>>(fines);

            var helper = new PaginationHelper<FineResponseDTO>();
            return helper.GetPaginatedData(
                source: dtoList,
                page: filter.Page,
                pageSize: filter.PageSize,
                sortField: filter.SortField,
                sortOrder: filter.SortOrder,
                filterFunc: f =>
                    (string.IsNullOrEmpty(filter.PlateNumber) || f.PlateNumber?.Contains(filter.PlateNumber) == true) &&
                    (string.IsNullOrEmpty(filter.Search) || (
                        f.RecipientFullName?.Contains(filter.Search, StringComparison.OrdinalIgnoreCase) == true ||
                        f.FineReason?.Contains(filter.Search, StringComparison.OrdinalIgnoreCase) == true
                    )) &&
                    (!filter.FromDate.HasValue || f.FineDate >= filter.FromDate.Value) &&
                    (!filter.ToDate.HasValue || f.FineDate <= filter.ToDate.Value)
            );
        }

        public async Task<PaginationResult<FineResponseDTO>> GetFinesCreatedByPoliceAsync(string policeId, FineFilterDTO filter)
        {
            var fines = await _repo.GetFinesCreatedByPoliceAsync(policeId);
            var result = _mapper.Map<List<FineResponseDTO>>(fines);

            var helper = new PaginationHelper<FineResponseDTO>();
            return helper.GetPaginatedData(
                source: result,
                page: filter.Page,
                pageSize: filter.PageSize,
                sortField: filter.SortField,
                sortOrder: filter.SortOrder,
                filterFunc: f =>
                    (string.IsNullOrEmpty(filter.PlateNumber) || f.PlateNumber?.Contains(filter.PlateNumber) == true) &&
                    (string.IsNullOrEmpty(filter.Search) || (
                        f.PlateNumber?.Contains(filter.Search, StringComparison.OrdinalIgnoreCase) == true ||
                        f.RecipientFullName?.Contains(filter.Search, StringComparison.OrdinalIgnoreCase) == true ||
                        f.FineReason?.Contains(filter.Search, StringComparison.OrdinalIgnoreCase) == true
                    )) &&
                    (!filter.FromDate.HasValue || f.FineDate >= filter.FromDate.Value) &&
                    (!filter.ToDate.HasValue || f.FineDate <= filter.ToDate.Value)
            );
        }

        public async Task<PaginationResult<FineResponseDTO>> GetAllFinesAsync(FineFilterDTO filter)
        {
            var allFines = await _repo.GetAllFinesAsync();

            var fineDtos = allFines.Select(f => new FineResponseDTO
            {
                IDPK_Fine = f.IDPK_Fine,
                FineAmount = f.FineAmount,
                FineReason = f.FineReason,
                FineDate = f.FineDate,
                PlateNumber = f.Vehicle?.PlateNumber,
                PoliceFullName = f.PoliceOfficer != null ? $"{f.PoliceOfficer.FirstName} {f.PoliceOfficer.LastName}" : null,
                RecipientFullName = f.FineRecipient != null ? $"{f.FineRecipient.FirstName} {f.FineRecipient.LastName}" : null
            }).ToList();

            bool validDateRange = true;
            if (filter.FromDate.HasValue && filter.ToDate.HasValue)
            {
                validDateRange = filter.FromDate.Value.Date <= filter.ToDate.Value.Date;
            }

            var filteredFines = fineDtos.Where(f =>
                (string.IsNullOrEmpty(filter.PlateNumber) || (f.PlateNumber != null && f.PlateNumber.Contains(filter.PlateNumber))) &&

                (!filter.FromDate.HasValue || f.FineDate.Date >= filter.FromDate.Value.Date) &&
                (!filter.ToDate.HasValue || f.FineDate.Date <= filter.ToDate.Value.Date) &&
                validDateRange
            ).ToList();

            Func<FineResponseDTO, bool>? filterFunc = null;
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var searchLower = filter.Search.ToLower();
                filterFunc = f =>
                    (!string.IsNullOrEmpty(f.PlateNumber) && f.PlateNumber.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(f.RecipientFullName) && f.RecipientFullName.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(f.FineReason) && f.FineReason.ToLower().Contains(searchLower)) ||
                    f.FineAmount.ToString().Contains(searchLower);
            }

            if (filterFunc is not null)
                filteredFines = filteredFines.Where(filterFunc).ToList();

            var helper = new PaginationHelper<FineResponseDTO>();
            var result = helper.GetPaginatedData(
                filteredFines,
                filter.Page,
                filter.PageSize,
                filter.SortField,
                filter.SortOrder
            );

            return result;
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
