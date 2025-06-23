using DTO.InspectionDTO;
using Entities.Models;
using System;
using System.Threading.Tasks;

namespace DAL.Contracts
{
    public interface IInspectionRequestRepository : IRepository<Auto_InspectionRequests>
    {
        Task<int> CountInspectionsByDateAndDirectoryAsync(Guid directoryId, DateTime date);
        Task<bool> HasPendingRequestAsync(Guid vehicleId);

        Task<List<MyInspectionRequestDTO>> GetRequestsByUserAsync(string userId);
    }
}
