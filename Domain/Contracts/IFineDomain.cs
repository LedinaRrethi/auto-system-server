using DTO;
using DTO.FineDTO;
using DTO.VehicleDTO;
using Helpers.Pagination;

namespace Domain.Contracts
{
    public interface IFineDomain
    {
        Task<bool> CreateFineAsync(FineCreateDTO dto, string policeId, string ip);
        Task<PaginationResult<FineResponseDTO>> GetMyFinesAsync(string userId, FineFilterDTO filter);
        Task<PaginationResult<FineResponseDTO>> GetFinesCreatedByPoliceAsync(string policeId, FineFilterDTO filter);
        Task<PaginationResult<FineResponseDTO>> GetAllFinesAsync(string userId , FineFilterDTO filter);
        Task<object?> GetRecipientDetailsByPlateAsync(string plate);
        Task<int> GetFinesCountAsync(string policeId);
        Task<int> GetFinesCountForUserAsync(string userId);
    }
}
