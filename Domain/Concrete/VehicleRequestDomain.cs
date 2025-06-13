using AutoMapper;
using DAL.Contracts;
using DAL.UoW;
using Domain.Contracts;
using DTO.VehicleDTO;
using DTO.VehicleRequest;
using Entities.Models;
using Helpers.Enumerations;
using Microsoft.AspNetCore.Http;
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

        public async Task RegisterVehicleAsync(VehicleRegisterDTO dto, string userId)
        {
            var hasPending = await _vehicleRequestRepository.HasPendingRequestForUserAsync(userId, ChangeRequestType.Register);
            if (hasPending)
                throw new Exception("You already have a pending registration request.");


            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var vehicle = _mapper.Map<Auto_Vehicles>(dto);
                vehicle.IDPK_Vehicle = Guid.NewGuid();
                vehicle.IDFK_Owner = userId;
                vehicle.Status = VehicleStatus.Pending;
                vehicle.Invalidated = 0;
                vehicle.CreatedBy = userId;
                vehicle.CreatedOn = DateTime.UtcNow;
                vehicle.CreatedIp = GetCurrentIp();

                await _vehicleRequestRepository.AddVehicleAsync(vehicle);

                var request = new Auto_VehicleChangeRequests
                {
                    IDPK_ChangeRequest = Guid.NewGuid(),
                    IDFK_Vehicle = vehicle.IDPK_Vehicle,
                    IDFK_Requester = userId,
                    RequestType = ChangeRequestType.Register,
                    RequestDataJson = JsonSerializer.Serialize(dto),
                    CurrentDataSnapshotJson = string.Empty,
                    Status = ChangeRequestStatus.Pending,
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow
                };

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
            var pendingExists = await _vehicleRequestRepository.HasPendingRequestForVehicleAsync(vehicleId);
            if (pendingExists)
                throw new Exception("A pending request already exists for this vehicle.");


            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var vehicle = await _vehicleRequestRepository.GetVehicleByIdAsync(vehicleId);
                if (vehicle == null || vehicle.IDFK_Owner != userId)
                    throw new Exception("Vehicle not found or not owned by user.");

                var snapshot = JsonSerializer.Serialize(vehicle);

                var request = new Auto_VehicleChangeRequests
                {
                    IDPK_ChangeRequest = Guid.NewGuid(),
                    IDFK_Vehicle = vehicleId,
                    IDFK_Requester = userId,
                    RequestType = ChangeRequestType.Update,
                    RequestDataJson = JsonSerializer.Serialize(dto),
                    CurrentDataSnapshotJson = snapshot,
                    Status = ChangeRequestStatus.Pending,
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow
                };

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

            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var vehicle = await _vehicleRequestRepository.GetVehicleByIdAsync(vehicleId);
                if (vehicle == null || vehicle.IDFK_Owner != userId)
                    throw new Exception("Vehicle not found or not owned by user.");

                var snapshot = JsonSerializer.Serialize(vehicle);

                var request = new Auto_VehicleChangeRequests
                {
                    IDPK_ChangeRequest = Guid.NewGuid(),
                    IDFK_Vehicle = vehicleId,
                    IDFK_Requester = userId,
                    RequestType = ChangeRequestType.Delete,
                    RequestDataJson = string.Empty,
                    CurrentDataSnapshotJson = snapshot,
                    Status = ChangeRequestStatus.Pending,
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow
                };

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

        public async Task<List<Auto_VehicleChangeRequests>> GetMyRequestsAsync(string userId)
        {
            return await _vehicleRequestRepository.GetRequestsByUserAsync(userId);
        }
    }
}
