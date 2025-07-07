using DTO;
using DTO.InspectionDTO;
using Entities.Models;
using Helpers.Pagination;
using System;
using System.Threading.Tasks;

namespace DAL.Contracts
{
    public interface IInspectionRequestRepository : IRepository<Auto_InspectionRequests>
    {
        Task<int> CountInspectionsByDateAndDirectoryAsync(Guid directoryId, DateTime date);
        Task<bool> HasPendingRequestAsync(Guid vehicleId);

        //Task<List<MyInspectionRequestDTO>> GetRequestsByUserAsync(string userId);

        Task<PaginationResult<MyInspectionRequestDTO>> GetCurrentUserPagedInspectionRequestsAsync(string userId, PaginationDTO dto);

        Task<Dictionary<string, int>> CountInspectionsByStatusForSpecialistAsync(Guid directoryId);
        Task<Dictionary<string, int>> CountInspectionRequestsByUserAsync(string userId);
    }
}
