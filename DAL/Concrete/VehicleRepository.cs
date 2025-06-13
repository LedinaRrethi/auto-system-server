using DAL.Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Concrete
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly AutoSystemDbContext _context;
        public VehicleRepository(AutoSystemDbContext context)
        {
            _context = context;
        }

        public async Task AddVehicleAsync (Auto_Vehicles vehicle)
        {
            await _context.Auto_Vehicles.AddAsync(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Auto_Vehicles>> GetVehiclesByOwnerAsync(string userId)
        {
            return await _context.Auto_Vehicles
                .Where(v => v.IDFK_Owner == userId && v.Invalidated == 0)
                .ToListAsync();
        }
    }
}
