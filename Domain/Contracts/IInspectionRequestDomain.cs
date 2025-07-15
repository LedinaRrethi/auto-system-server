using DTO;
using DTO.InspectionDTO;
using Helpers.Pagination;

namespace Domain.Contracts
{
    public interface IInspectionRequestDomain
    {
        Task<bool> CreateInspectionRequestAsync(InspectionRequestCreateDTO dto);
        Task<PaginationResult<MyInspectionRequestDTO>> GetCurrentUserPagedInspectionRequestsAsync(PaginationDTO dto);
        Task<string?> GetInspectionDocumentBase64Async(Guid documentId);

        Task<Dictionary<string, int>> GetInspectionStatusCountAsync(Guid directoryId);
        Task<Dictionary<string, int>> GetInspectionRequestStatusForUserAsync(string userId);

    }
}
