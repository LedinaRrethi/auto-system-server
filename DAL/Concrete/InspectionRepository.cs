using DAL.Contracts;
using Entities.Models;
using Helpers.Enumerations;
using Microsoft.EntityFrameworkCore;

namespace DAL.Concrete
{
    public class InspectionRepository : BaseRepository<Auto_Inspections>, IInspectionRepository
    {
        private readonly AutoSystemDbContext _context;

        public InspectionRepository(AutoSystemDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Auto_InspectionRequests?> GetRequestByIdAsync(Guid requestId)
        {
            return await _context.Auto_InspectionRequests
                .FirstOrDefaultAsync(x => x.IDPK_InspectionRequest == requestId && x.Invalidated == 0);
        }

        public async Task<List<Auto_InspectionRequests>> GetRequestsBySpecialistAsync(string specialistId)
        {
            var specialist = await _context.Users
                .OfType<Auto_Users>()
                .Where(u => u.Id == specialistId)
                .Select(u => new { u.IDFK_Directory })
                .FirstOrDefaultAsync();

            if (specialist == null || specialist.IDFK_Directory == null)
                return new List<Auto_InspectionRequests>();

            return await _context.Auto_InspectionRequests
                .Include(r => r.Vehicle)
                .Include(r => r.Requester)
                .Where(r => r.IDFK_Directory == specialist.IDFK_Directory && r.Invalidated == 0)
                .ToListAsync();
        }

        public Task<List<Auto_Vehicles>> GetVehiclesByUserIdAsync(string userId)
        {
            return _context.Auto_Vehicles
                .Where(v => v.IDFK_Owner == userId && v.Invalidated == 0 && v.Status == VehicleStatus.Approved)
                .ToListAsync();
        }

    }
}
