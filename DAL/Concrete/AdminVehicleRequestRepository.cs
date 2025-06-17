using DAL.Contracts;
using DTO.VehicleRequest;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class AdminVehicleRequestRepository : IAdminVehicleRequestRepository
    {
        private readonly AutoSystemDbContext _context;

        public AdminVehicleRequestRepository(AutoSystemDbContext context)
        {
            _context = context;
        }

        public async Task<List<Auto_VehicleChangeRequests>> GetAllRequestsAsync()
        {
            return await _context.Auto_VehicleChangeRequests
                .Include(r => r.Vehicle)
                .Include(r => r.Requester)
                .ToListAsync();
        }

        public async Task<Auto_VehicleChangeRequests?> GetRequestByIdAsync(Guid requestId)
        {
            return await _context.Auto_VehicleChangeRequests
                .Include(r => r.Vehicle)
                .FirstOrDefaultAsync(r => r.IDPK_ChangeRequest == requestId);
        }

        public async Task<Auto_Vehicles?> GetVehicleByIdAsync(Guid vehicleId)
        {
            return await _context.Auto_Vehicles.FindAsync(vehicleId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
