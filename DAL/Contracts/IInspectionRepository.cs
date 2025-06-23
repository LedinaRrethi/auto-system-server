using Entities.Models;

namespace DAL.Contracts
{
    public interface IInspectionRepository : IRepository<Auto_Inspections>
    {
        Task<Auto_InspectionRequests?> GetRequestByIdAsync(Guid requestId);
        Task<List<Auto_InspectionRequests>> GetRequestsBySpecialistAsync(string specialistId);

        Task<List<Auto_Vehicles>> GetVehiclesByUserIdAsync(string userId);

    }
}
