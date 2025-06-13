using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contracts
{
    public interface IVehicleRepository
    {
        Task AddVehicleAsync(Auto_Vehicles vehicle);
        Task<List<Auto_Vehicles>> GetVehiclesByOwnerAsync(string userId);
    }
}
