using DTO;
using DTO.InspectionDTO;
using DTO.VehicleDTO;
using Helpers.Pagination;

namespace Domain.Contracts
{
    public interface IInspectionDomain
    {
        Task<PaginationResult<InspectionRequestListDTO>> GetMyRequestsAsync(string userId, PaginationDTO dto);

        //Task<bool> ReviewInspectionAsync(InspectionReviewDTO dto);
        //Task<bool> UploadDocumentsAsync(List<InspectionDocumentUploadDTO> documents);
        //Task<List<InspectionDocumentDTO>> GetDocumentsAsync(Guid requestId);
        //Task<bool> DeleteDocumentAsync(Guid docId);

        Task<List<VehicleDTO>> GetMyVehiclesAsync(string userId);
    }
}
