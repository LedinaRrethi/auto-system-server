using DTO.InspectionDTO;
using DTO.VehicleDTO;

namespace Domain.Contracts
{
    public interface IInspectionDomain
    {
        Task<List<InspectionRequestCreateDTO>> GetMyRequestsAsync();
        Task<bool> ReviewInspectionAsync(InspectionReviewDTO dto);
        Task<bool> UploadDocumentsAsync(List<InspectionDocumentUploadDTO> documents);
        Task<List<InspectionDocDTO>> GetDocumentsAsync(Guid requestId);
        Task<bool> DeleteDocumentAsync(Guid docId);

        Task<List<VehicleDTO>> GetMyVehiclesAsync(string userId);
    }
}
