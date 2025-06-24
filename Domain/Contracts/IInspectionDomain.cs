using DTO.InspectionDTO;
using DTO.VehicleDTO;

namespace Domain.Contracts
{
    public interface IInspectionDomain
    {
        Task<List<InspectionRequestListDTO>> GetMyRequestsAsync(string userId);
        //Task<bool> ReviewInspectionAsync(InspectionReviewDTO dto);
        //Task<bool> UploadDocumentsAsync(List<InspectionDocumentUploadDTO> documents);
        //Task<List<InspectionDocumentDTO>> GetDocumentsAsync(Guid requestId);
        //Task<bool> DeleteDocumentAsync(Guid docId);

        Task<List<VehicleDTO>> GetMyVehiclesAsync(string userId);
    }
}
