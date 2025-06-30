using DAL.Contracts;
using DAL.UoW;
using Domain.Contracts;
using DTO;
using DTO.VehicleRequest;
using Entities.Models;
using Helpers.Enumerations;
using Helpers.Pagination;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Domain.Concrete
{
    public class AdminVehicleRequestDomain : DomainBase, IAdminVehicleRequestDomain
    {
        public AdminVehicleRequestDomain(IUnitOfWork unitOfWork, IHttpContextAccessor accessor)
            : base(unitOfWork, null, accessor) { }

        private IAdminVehicleRequestRepository _adminRequestRepo => _unitOfWork.GetRepository<IAdminVehicleRequestRepository>();
        private IRepository<Auto_Vehicles> _vehicleRepo => _unitOfWork.GetRepository<IRepository<Auto_Vehicles>>();

        public async Task<PaginationResult<VehicleRequestListDTO>> GetAllRequestsAsync(PaginationDTO dto)
        {
            var requests = await _adminRequestRepo.GetAllRequestsAsync();

            var mapped = requests.Select(r => new VehicleRequestListDTO
            {
                IDPK_ChangeRequest = r.IDPK_ChangeRequest,
                IDFK_Vehicle = r.IDFK_Vehicle,
                RequestType = r.RequestType,
                RequestDataJson = r.RequestDataJson,
                CurrentDataSnapshotJson = r.CurrentDataSnapshotJson,
                PlateNumber = r.Vehicle?.PlateNumber,
                Status = r.Status,
                CreatedOn = r.CreatedOn
            }).ToList();

            var helper = new PaginationHelper<VehicleRequestListDTO>();
            return helper.GetPaginatedData(
                mapped,
                dto.Page,
                dto.PageSize,
                dto.SortField ?? nameof(VehicleRequestListDTO.CreatedOn),
                dto.SortOrder ?? "desc",
                string.IsNullOrWhiteSpace(dto.Search)
                    ? null
                    : (Func<VehicleRequestListDTO, bool>)(r =>
                        !string.IsNullOrEmpty(r.PlateNumber) &&
                        r.PlateNumber.Contains(dto.Search, StringComparison.OrdinalIgnoreCase))
            );
        }

        public async Task<bool> UpdateRequestStatusAsync(Guid requestId, VehicleChangeStatusDTO dto)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var request = await _adminRequestRepo.GetRequestByIdAsync(requestId);
                if (request == null || request.Status != VehicleStatus.Pending)
                    return false;

                var vehicle = request.Vehicle;
                if (vehicle == null)
                    return false;

                request.Status = dto.NewStatus;
                request.AdminComment = dto.AdminComment;
                SetAuditOnUpdate(request);
                await _adminRequestRepo.UpdateAsync(request);

                if (dto.NewStatus == VehicleStatus.Approved)
                {
                    switch (request.RequestType)
                    {
                        case ChangeRequestType.Register:
                            vehicle.Status = VehicleStatus.Approved;
                            break;

                        case ChangeRequestType.Update:
                            var updateDto = JsonSerializer.Deserialize<VehicleUpdateDTO>(request.RequestDataJson);
                            if (updateDto != null)
                            {
                                vehicle.PlateNumber = updateDto.PlateNumber;
                                vehicle.Color = updateDto.Color;
                                vehicle.SeatCount = updateDto.SeatCount;
                                vehicle.DoorCount = updateDto.DoorCount;
                                vehicle.ChassisNumber = updateDto.ChassisNumber;
                            }
                            vehicle.Status = VehicleStatus.Approved;
                            break;

                        case ChangeRequestType.Delete:
                            vehicle.Invalidated = 1;
                            break;
                    }

                    SetAuditOnUpdate(vehicle);
                    await _vehicleRepo.UpdateAsync(vehicle);
                }

                await _unitOfWork.CommitAsync();
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
