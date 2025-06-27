using DTO;
using DTO.VehicleDTO;
using DTO.VehicleRequest;
using Entities.Models;
using Helpers.Pagination;

namespace Domain.Contracts
{
    public interface IVehicleRequestDomain
    {
        Task RegisterVehicleAsync(Guid vehicleId , VehicleRegisterDTO dto, string userId);
        Task RequestVehicleUpdateAsync(Guid vehicleId, VehicleRegisterDTO dto, string userId);
        Task RequestVehicleDeletionAsync(Guid vehicleId, string userId);
        Task<PaginationResult<VehicleRequestListDTO>> GetMyRequestsAsync(string userId , PaginationDTO dto);

    }
}
