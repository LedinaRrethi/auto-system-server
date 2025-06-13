using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.VehicleDTO;
using DTO.VehicleRequest;
using Entities.Models;

namespace Domain.Contracts
{
    public interface IVehicleRequestDomain
    {
        Task RegisterVehicleAsync(VehicleRegisterDTO dto, string userId);
        Task RequestVehicleUpdateAsync(Guid vehicleId, VehicleRegisterDTO dto, string userId);
        Task RequestVehicleDeletionAsync(Guid vehicleId, string requesterComment, string userId);
        Task<List<Auto_VehicleChangeRequests>> GetMyRequestsAsync(string userId);
    }
}
