using Entities.Models;

namespace DAL.Contracts
{
    public interface IVehicleRequestRepository
    {
        Task<List<Auto_VehicleChangeRequests>> GetRequestsByUserAsync(string userId);
        Task AddVehicleAsync(Auto_Vehicles vehicle);
        Task AddRequestAsync(Auto_VehicleChangeRequests request);
        Task<Auto_Vehicles?> GetVehicleByIdAsync(Guid vehicleId);
        Task SaveChangesAsync();
    }
}
