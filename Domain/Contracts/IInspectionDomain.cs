using DTO.InspectionDTO;

namespace Domain.Contracts
{
    public interface IInspectionDomain
    {
        Task<List<InspectionRequestCreateDTO>> GetMyRequestsAsync();
        Task<bool> ReviewInspectionAsync(InspectionReviewDTO dto);
        Task<bool> UploadDocumentsAsync(List<InspectionDocumentUploadDTO> documents);
        Task<List<InspectionDocDTO>> GetDocumentsAsync(Guid requestId);
        Task<bool> DeleteDocumentAsync(Guid docId);
    }
}
