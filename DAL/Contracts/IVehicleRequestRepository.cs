using Entities.Models;
using Helpers.Enumerations;

namespace DAL.Contracts
{
    public interface IVehicleRequestRepository
    {
        Task<List<Auto_VehicleChangeRequests>> GetRequestsByUserAsync(string userId);
        Task AddVehicleAsync(Auto_Vehicles vehicle);
        Task AddRequestAsync(Auto_VehicleChangeRequests request);
        Task<Auto_Vehicles?> GetVehicleByIdAsync(Guid vehicleId);

        Task<bool> HasPendingRequestForVehicleAsync(Guid vehicleId);
        Task<bool> HasPendingRequestForUserAsync(string userId, ChangeRequestType type);
        Task SaveChangesAsync();
    }
}
