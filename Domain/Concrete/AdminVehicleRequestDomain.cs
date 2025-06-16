using AutoMapper;
using DAL.Contracts;
using DAL.UoW;
using Domain.Contracts;
using DTO.VehicleRequest;
using Entities.Models;
using Helpers.Enumerations;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Domain.Concrete
{
    public class AdminVehicleRequestDomain : DomainBase, IAdminVehicleRequestDomain
    {
        private readonly IAdminVehicleRequestRepository _adminRequestRepo;

        public AdminVehicleRequestDomain(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor accessor)
            : base(unitOfWork, mapper, accessor)
        {
            _adminRequestRepo = unitOfWork.GetRepository<IAdminVehicleRequestRepository>();
        }

        public async Task<List<VehicleRequestListDTO>> GetAllRequestsAsync()
        {
            var requests = await _adminRequestRepo.GetAllRequestsAsync();

            return requests.Select(r => new VehicleRequestListDTO
            {
                IDPK_ChangeRequest = r.IDPK_ChangeRequest,
                IDFK_Vehicle = r.IDFK_Vehicle,
                PlateNumber = r.Vehicle?.PlateNumber,
                RequestType = r.RequestType,
                Status = r.Status,
                CreatedOn = r.CreatedOn
            }).ToList();
        }

        public async Task<bool> UpdateRequestStatusAsync(Guid requestId, VehicleChangeStatusDTO dto)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var request = await _adminRequestRepo.GetRequestByIdAsync(requestId);
                if (request == null || request.Status != ChangeRequestStatus.Pending)
                    return false;

                var vehicle = await _adminRequestRepo.GetVehicleByIdAsync(request.IDFK_Vehicle);

                request.Status = dto.NewStatus;
                request.AdminComment = dto.AdminComment;

                if (dto.NewStatus == ChangeRequestStatus.Approved)
                {
                    switch (request.RequestType)
                    {
                        case ChangeRequestType.Register when vehicle != null:
                            vehicle.Status = VehicleStatus.Approved;
                            break;

                        case ChangeRequestType.Update when vehicle != null:
                            var updateDto = JsonSerializer.Deserialize<VehicleUpdateDTO>(request.RequestDataJson);
                            if (updateDto != null)
                            {
                                vehicle.PlateNumber = updateDto.PlateNumber;
                                vehicle.Color = updateDto.Color;
                            
                            }
                            break;

                        case ChangeRequestType.Delete when vehicle != null:
                            vehicle.Invalidated = 1;
                            break;
                    }
                }

                await _adminRequestRepo.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
