using DAL.Concrete;
using DAL.Contracts;
using DTO.VehicleRequest;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class AdminVehicleRequestRepository : BaseRepository<Auto_VehicleChangeRequests>, IAdminVehicleRequestRepository
    {
        private readonly AutoSystemDbContext _context;

        public AdminVehicleRequestRepository(AutoSystemDbContext context)  : base(context)
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
            return await _context.Auto_Vehicles.FirstOrDefaultAsync(v => v.IDPK_Vehicle == vehicleId);
            

        }
        public async Task UpdateAsync(Auto_VehicleChangeRequests request)
        {
            _context.Auto_VehicleChangeRequests.Update(request);
            await Task.CompletedTask; 
        }


    }
}
