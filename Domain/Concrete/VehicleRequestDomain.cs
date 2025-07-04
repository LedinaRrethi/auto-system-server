using AutoMapper;
using DAL.Contracts;
using DAL.UoW;
using Domain.Contracts;
using DTO;
using DTO.VehicleDTO;
using DTO.VehicleRequest;
using Entities.Models;
using Helpers.Enumerations;
using Helpers.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace Domain.Concrete
{
    public class VehicleRequestDomain : DomainBase, IVehicleRequestDomain
    {
        private readonly IVehicleRequestRepository _vehicleRequestRepository;

        public VehicleRequestDomain(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor accessor)
            : base(unitOfWork, mapper, accessor)
        {
            _vehicleRequestRepository = unitOfWork.GetRepository<IVehicleRequestRepository>();
        }

        public async Task RegisterVehicleAsync(Guid vehicleId, VehicleRegisterDTO dto, string userId)
        {
            if (await _vehicleRequestRepository.HasPendingRequestForVehicleAsync(vehicleId))
                throw new Exception("A pending request already exists for this vehicle.");

            if (await _vehicleRequestRepository.PlateNumberExistsAsync(dto.PlateNumber))
                throw new Exception("This plate number is already registered.");

            if (await _vehicleRequestRepository.ChassisNumberExistsAsync(dto.ChassisNumber))
                throw new Exception("This chassis number is already registered.");


            using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var vehicle = _mapper.Map<Auto_Vehicles>(dto);
                vehicle.IDPK_Vehicle = vehicleId;
                vehicle.IDFK_Owner = userId;
                vehicle.Status = VehicleStatus.Pending;
                vehicle.Invalidated = 0;
                SetAuditOnCreate(vehicle);

                await _vehicleRequestRepository.AddVehicleAsync(vehicle);

                var request = new Auto_VehicleChangeRequests
                {
                    IDPK_ChangeRequest = Guid.NewGuid(),
                    IDFK_Vehicle = vehicleId,
                    IDFK_Requester = userId,
                    RequestType = ChangeRequestType.Register,
                    RequestDataJson = JsonSerializer.Serialize(dto),
                    CurrentDataSnapshotJson = string.Empty,
                    Status = VehicleStatus.Pending
                };
                SetAuditOnCreate(request);

                await _vehicleRequestRepository.AddRequestAsync(request);
                await _vehicleRequestRepository.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task RequestVehicleUpdateAsync(Guid vehicleId, VehicleRegisterDTO dto, string userId)
        {
            var vehicle = await _vehicleRequestRepository.GetVehicleByIdAsync(vehicleId);
            if (vehicle == null || vehicle.IDFK_Owner != userId)
                throw new Exception("Vehicle not found or not owned by user.");

            var hasPending = await _vehicleRequestRepository.HasPendingRequestForVehicleAsync(vehicleId);
            if (hasPending)
                throw new Exception("This vehicle already has a pending request.");

            using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var snapshot = JsonSerializer.Serialize(vehicle);

                var request = new Auto_VehicleChangeRequests
                {
                    IDPK_ChangeRequest = Guid.NewGuid(),
                    IDFK_Vehicle = vehicleId,
                    IDFK_Requester = userId,
                    RequestType = ChangeRequestType.Update,
                    RequestDataJson = JsonSerializer.Serialize(dto),
                    CurrentDataSnapshotJson = snapshot,
                    Status = VehicleStatus.Pending
                };
                SetAuditOnCreate(request);

                await _vehicleRequestRepository.AddRequestAsync(request);
                await _vehicleRequestRepository.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task RequestVehicleDeletionAsync(Guid vehicleId, string userId)
        {
            var vehicle = await _vehicleRequestRepository.GetVehicleByIdAsync(vehicleId);
            if (vehicle == null || vehicle.IDFK_Owner != userId)
                throw new Exception("Vehicle not found or not owned by user.");

            var hasPending = await _vehicleRequestRepository.HasPendingRequestForVehicleAsync(vehicleId);
            if (hasPending)
                throw new Exception("This vehicle already has a pending request.");

            using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var snapshot = JsonSerializer.Serialize(vehicle);

                var request = new Auto_VehicleChangeRequests
                {
                    IDPK_ChangeRequest = Guid.NewGuid(),
                    IDFK_Vehicle = vehicleId,
                    IDFK_Requester = userId,
                    RequestType = ChangeRequestType.Delete,
                    RequestDataJson = string.Empty,
                    CurrentDataSnapshotJson = snapshot,
                    Status = VehicleStatus.Pending
                };
                SetAuditOnCreate(request);

                await _vehicleRequestRepository.AddRequestAsync(request);
                await _vehicleRequestRepository.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<PaginationResult<VehicleDTO>> GetMyRequestsAsync(string userId, PaginationDTO dto)
        {
            var myRequests = await _vehicleRequestRepository.GetRequestsByUserAsync(userId);

            var mapped = myRequests
                .Select(r =>
                {
                    if (string.IsNullOrEmpty(r.RequestDataJson) && r.RequestType == ChangeRequestType.Delete)
                    {
                        return new VehicleDTO
                        {
                            IDPK_Vehicle = r.IDFK_Vehicle,
                            PlateNumber = r.Vehicle?.PlateNumber ?? "",
                            Color = r.Vehicle?.Color ?? "",
                            SeatCount = r.Vehicle?.SeatCount ?? 0,
                            DoorCount = r.Vehicle?.DoorCount ?? 0,
                            ChassisNumber = r.Vehicle?.ChassisNumber ?? "",
                            Status = r.Status,
                            CreatedOn = r.CreatedOn,
                            ApprovalComment = r.AdminComment
                        };
                    }

                    try
                    {
                        var deserialized = JsonSerializer.Deserialize<VehicleDTO>(r.RequestDataJson);
                        if (deserialized != null)
                        {
                            deserialized.IDPK_Vehicle = r.IDFK_Vehicle;
                            deserialized.Status = r.Status;
                            deserialized.CreatedOn = r.CreatedOn;
                            deserialized.ApprovalComment = r.AdminComment;
                        }
                        return deserialized;
                    }
                    catch
                    {
                        return null;
                    }
                })
                .Where(x => x != null)
                .ToList();

            var helper = new PaginationHelper<VehicleDTO>();

            return helper.GetPaginatedData(
                mapped!,
                dto.Page,
                dto.PageSize,
                dto.SortField ?? "CreatedOn",
                dto.SortOrder ?? "desc",
                string.IsNullOrWhiteSpace(dto.Search)
                    ? null
                    : (Func<VehicleDTO, bool>)(r =>
                        (!string.IsNullOrEmpty(r.PlateNumber) && r.PlateNumber.Contains(dto.Search, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrEmpty(r.Color) && r.Color.Contains(dto.Search, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrEmpty(r.ChassisNumber) && r.ChassisNumber.Contains(dto.Search, StringComparison.OrdinalIgnoreCase)) ||
                        r.Status.ToString().Contains(dto.Search, StringComparison.OrdinalIgnoreCase) ||
                        r.SeatCount.ToString().Contains(dto.Search, StringComparison.OrdinalIgnoreCase) ||
                        r.DoorCount.ToString().Contains(dto.Search, StringComparison.OrdinalIgnoreCase)
                    ));
        }


        public async Task<VehicleEditDTO> GetVehicleForEditAsync(Guid vehicleId, string userId)
        {
            var vehicle = await _vehicleRequestRepository.GetVehicleByIdAsync(vehicleId);

            if (vehicle == null || vehicle.IDFK_Owner != userId)
                throw new Exception("Vehicle not found or access denied.");

            var mapped = _mapper.Map<VehicleEditDTO>(vehicle);

            return mapped;
        }

    }
}
