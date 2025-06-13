using DTO.VehicleDTO;
using DTO.VehicleRequest;
using Entities.Models;

namespace Domain.Contracts
{
    public interface IVehicleRequestDomain
    {
        Task RegisterVehicleAsync(Guid vehicleId , VehicleRegisterDTO dto, string userId);
        Task RequestVehicleUpdateAsync(Guid vehicleId, VehicleRegisterDTO dto, string userId);
        Task RequestVehicleDeletionAsync(Guid vehicleId, string userId);

        Task<List<VehicleRequestListDTO>> GetMyRequestsAsync(string userId);

    }
}
