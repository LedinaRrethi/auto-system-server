using DTO.VehicleRequest;
using Entities.Models;

namespace DAL.Contracts
{
    public interface IAdminVehicleRequestRepository
    {
        Task<List<Auto_VehicleChangeRequests>> GetAllRequestsAsync();
        Task<Auto_VehicleChangeRequests?> GetRequestByIdAsync(Guid requestId);
        Task<Auto_Vehicles?> GetVehicleByIdAsync(Guid vehicleId);
        Task SaveChangesAsync();
        Task UpdateAsync(Auto_VehicleChangeRequests request);
        Task<Dictionary<string, int>> CountVehicleRequestStatusAsync();
    }
}
