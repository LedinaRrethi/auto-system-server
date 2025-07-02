using DTO;
using DTO.InspectionDTO;
using DTO.VehicleDTO;
using Helpers.Pagination;

namespace Domain.Contracts
{
    public interface IInspectionDomain
    {
        Task<PaginationResult<InspectionRequestListDTO>> GetMyRequestsAsync(string userId, PaginationDTO dto);

        Task<bool> ApproveInspectionAsync(InspectionApprovalDTO dto, string? userId, string ip);

        Task<List<VehicleDTO>> GetMyVehiclesAsync(string userId);

    }
}
