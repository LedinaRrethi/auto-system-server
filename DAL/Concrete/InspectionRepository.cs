using DAL.Contracts;
using Entities.Models;
using Helpers.Enumerations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DAL.Concrete
{
    public class InspectionRepository : BaseRepository<Auto_InspectionRequests>, IInspectionRepository
    {
        public InspectionRepository(AutoSystemDbContext context) : base(context)
        {
        }

        public async Task<int> CountInspectionsByDateAndDirectoryAsync(Guid directoryId, DateTime date)
        {
            return await _dbSet
                .CountAsync(r =>
                    r.IDFK_Directory == directoryId &&
                    r.RequestedDate.Date == date.Date &&
                    r.Invalidated == 0);
        }

        public async Task<bool> HasPendingRequestAsync(Guid vehicleId)
        {
            return await _dbSet
                .AnyAsync(r =>
                    r.IDFK_Vehicle == vehicleId &&
                    r.Status == InspectionStatus.Pending &&
                    r.Invalidated == 0);
        }
    }
}
