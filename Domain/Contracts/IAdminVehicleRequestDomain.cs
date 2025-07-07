using DTO;
using DTO.VehicleRequest;
using Helpers.Pagination;

namespace Domain.Contracts
{
    public interface IAdminVehicleRequestDomain
    {
        Task<PaginationResult<VehicleRequestListDTO>> GetAllRequestsAsync(PaginationDTO dto);
        Task<bool> UpdateRequestStatusAsync(Guid requestId, VehicleChangeStatusDTO dto);
        Task<Dictionary<string, int>> GetVehicleRequestCountAsync();
    }
}
