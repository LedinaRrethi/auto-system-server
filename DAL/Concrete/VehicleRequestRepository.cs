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
                .Where(r => r.IDFK_Requester == userId && r.Vehicle.Invalidated ==0)
                .Include(r => r.Vehicle)
                .GroupBy(r => r.IDFK_Vehicle)
                .Select(g => g.OrderByDescending(r => r.CreatedOn).First())
                .ToListAsync();
        }


        public async Task<Auto_VehicleChangeRequests?> GetChangeRequestByIdAsync(Guid requestId)
        {
            return await _context.Auto_VehicleChangeRequests
                .Include(r => r.Vehicle)
                .FirstOrDefaultAsync(r => r.IDPK_ChangeRequest == requestId);
        }

        public async Task<Auto_Vehicles?> GetVehicleByIdAsync(Guid vehicleId)
        {
            return await _context.Auto_Vehicles
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.IDPK_Vehicle == vehicleId);
        }

        public async Task AddVehicleAsync(Auto_Vehicles vehicle)
        {
            await _context.Auto_Vehicles.AddAsync(vehicle);
        }

        public async Task AddRequestAsync(Auto_VehicleChangeRequests request)
        {
            await _context.Auto_VehicleChangeRequests.AddAsync(request);
        }

        public async Task<bool> HasPendingRequestForVehicleAsync(Guid vehicleId)
        {
            return await _context.Auto_VehicleChangeRequests
                .AnyAsync(r => r.IDFK_Vehicle == vehicleId && r.Status == VehicleStatus.Pending);
        }

        public async Task<bool> PlateNumberExistsAsync(string plateNumber)
        {
            return await _context.Auto_Vehicles
                .AnyAsync(v => v.PlateNumber == plateNumber);
        }

        public async Task<bool> ChassisNumberExistsAsync(string chassisNumber)
        {
            return await _context.Auto_Vehicles
                .AnyAsync(v => v.ChassisNumber == chassisNumber );
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
