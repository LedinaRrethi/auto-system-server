using DAL.Contracts;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Helpers.Enumerations;

namespace DAL.Concrete
{
    public class InspectionRepository : IInspectionRepository
    {
        private readonly AutoSystemDbContext _context;

        public InspectionRepository(AutoSystemDbContext context)
        {
            _context = context;
        }

        public async Task<int> CountInspectionsByDateAndDirectoryAsync(Guid directoryId, DateTime date)
        {
            return await _context.Auto_InspectionRequests
                .CountAsync(r => r.IDFK_Directory == directoryId
                              && r.RequestedDate.Date == date.Date
                              && r.Invalidated == 0);
        }

        public async Task AddInspectionRequestAsync(Auto_InspectionRequests request)
        {
            await _context.Auto_InspectionRequests.AddAsync(request);
        }

        public async Task<bool> HasPendingRequestAsync(Guid vehicleId)
        {
            return await _context.Auto_InspectionRequests
                .AnyAsync(r =>
                    r.IDFK_Vehicle == vehicleId &&
                    r.Status == InspectionStatus.Pending &&
                    r.Invalidated == 0);
        }

        public Task SaveChangesAsync() => _context.SaveChangesAsync();
    }

}
