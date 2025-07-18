using DAL.Concrete;
using DAL.Contracts;
using DTO.VehicleRequest;
using Entities.Models;
using Helpers.Enumerations;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class AdminVehicleRequestRepository : BaseRepository<Auto_VehicleChangeRequests>, IAdminVehicleRequestRepository
    {
    
        public AdminVehicleRequestRepository(AutoSystemDbContext context)  : base(context)
        {
      
        }

        public async Task<List<Auto_VehicleChangeRequests>> GetAllRequestsAsync()
        {
            var allRequests = await _context.Auto_VehicleChangeRequests
                .Include(r => r.Vehicle)
                .Include(r => r.Requester)
                .OrderBy(r => r.Status)
                .ThenByDescending(r => r.CreatedOn)
                .ToListAsync();

            return allRequests;
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
        public async Task UpdateVehicleAsync(Auto_VehicleChangeRequests request)
        {
            _context.Auto_VehicleChangeRequests.Update(request);
            await Task.CompletedTask;
        }

        public async Task<Dictionary<string, int>> CountVehicleRequestStatusAsync()
        {
            return await _context.Auto_VehicleChangeRequests
                .GroupBy(r => r.Status)
                .Select(g => new { Status = g.Key.ToString(), Count = g.Count() })
                .ToDictionaryAsync(g => g.Status, g => g.Count);
        }
    }
}
