using Entities.Models;
using System;
using System.Threading.Tasks;

namespace DAL.Contracts
{
    public interface IInspectionRepository : IRepository<Auto_InspectionRequests>
    {
        Task<int> CountInspectionsByDateAndDirectoryAsync(Guid directoryId, DateTime date);
        Task<bool> HasPendingRequestAsync(Guid vehicleId);
    }
}
