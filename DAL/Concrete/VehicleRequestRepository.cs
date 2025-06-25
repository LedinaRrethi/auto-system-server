using DAL.Contracts;
using Entities.Models;
using Helpers.Enumerations;
using Microsoft.EntityFrameworkCore;

namespace DAL.Concrete
{
    public class VehicleRequestRepository : IVehicleRequestRepository
    {
        private readonly AutoSystemDbContext _context;

        public VehicleRequestRepository(AutoSystemDbContext context)
        {
            _context = context;
        }

        public async Task<List<Auto_VehicleChangeRequests>> GetRequestsByUserAsync(string userId)
        {
            return await _context.Auto_VehicleChangeRequests
                .Where(r => r.IDFK_Requester == userId)
                .Include(r => r.Vehicle)
                .ToListAsync();
        }

        public async Task AddVehicleAsync(Auto_Vehicles vehicle)
        {
            await _context.Auto_Vehicles.AddAsync(vehicle);
        }

        public async Task AddRequestAsync(Auto_VehicleChangeRequests request)
        {
            await _context.Auto_VehicleChangeRequests.AddAsync(request);
        }

        public async Task<Auto_Vehicles?> GetVehicleByIdAsync(Guid vehicleId)
        {
            return await _context.Auto_Vehicles
                .AsNoTracking() 
                .FirstOrDefaultAsync(v => v.IDPK_Vehicle == vehicleId);
        }

        public async Task<bool> HasPendingRequestForVehicleAsync(Guid vehicleId)
        {
            return await _context.Auto_VehicleChangeRequests
                .AnyAsync(r => r.IDFK_Vehicle == vehicleId && r.Status == ChangeRequestStatus.Pending);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
