using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contracts
{
    public interface IInspectionRepository
    {
        Task<int> CountInspectionsByDateAndDirectoryAsync(Guid directoryId, DateTime date);
        Task AddInspectionRequestAsync(Auto_InspectionRequests request);

        Task<bool> HasPendingRequestAsync(Guid vehicleId);

        Task SaveChangesAsync();
    }


}
