using DTO.VehicleRequest;

namespace Domain.Contracts
{
    public interface IAdminVehicleRequestDomain
    {
        Task<List<VehicleRequestListDTO>> GetAllRequestsAsync();
        Task<bool> UpdateRequestStatusAsync(Guid requestId, VehicleChangeStatusDTO dto);
    }
}
