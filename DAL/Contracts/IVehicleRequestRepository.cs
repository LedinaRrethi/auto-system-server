using DTO.VehicleDTO;
using Entities.Models;
using Helpers.Enumerations;

namespace DAL.Contracts
{
    public interface IVehicleRequestRepository
    {
        Task<List<Auto_Vehicles>> GetRequestsByUserAsync(string userId);
        Task<Auto_Vehicles?> GetVehicleByIdAsync(Guid vehicleId);
        Task<bool> HasPendingRequestForVehicleAsync(Guid vehicleId);
        Task AddVehicleAsync(Auto_Vehicles vehicle);
        Task AddRequestAsync(Auto_VehicleChangeRequests request);
        Task<bool> PlateNumberExistsAsync(string plateNumber);
        Task<bool> ChassisNumberExistsAsync(string chassisNumber);
        Task<Dictionary<string, int>> CountVehicleRequestStatusForUserAsync(string userId);
        Task<Auto_Vehicles?> GetVehicleByPlateAsync(string plateNumber);
        Task SaveChangesAsync();
    }
}
