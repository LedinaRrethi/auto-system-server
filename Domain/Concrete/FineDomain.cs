using AutoMapper;
using DAL.Contracts;
using Domain.Contracts;
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

        public FineDomain(IFineRepository repo, IMapper mapper, UserManager<Auto_Users> userManager)
        {
            _repo = repo;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<bool> CreateFineAsync(FineCreateDTO dto, string policeId, string ip)
        {
            var vehicle = await _repo.GetVehicleByPlateAsync(dto.PlateNumber);
            Auto_Users? owner = vehicle?.IDFK_Owner != null ? await _repo.GetUserByIdAsync(vehicle.IDFK_Owner) : null;

            var recipient = new Auto_FineRecipients
            {
                IDPK_FineRecipient = Guid.NewGuid(),
                IDFK_User = owner?.Id,
                FirstName = owner?.FirstName ?? dto.FirstName!,
                LastName = owner?.LastName ?? dto.LastName!,
                FatherName = owner?.FatherName ?? dto.FatherName,
                PhoneNumber = dto.PhoneNumber,
                PersonalId = dto.PersonalId,
                PlateNumber = dto.PlateNumber,
                CreatedBy = policeId,
                CreatedOn = DateTime.UtcNow,
                CreatedIp = ip
            };

            await _repo.AddFineRecipientAsync(recipient);
            await _repo.SaveChangesAsync();

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
            return true;
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

    }
}
