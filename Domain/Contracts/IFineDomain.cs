using DTO.FineDTO;
using DTO.VehicleDTO;
using Helpers.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IFineDomain
    {
        Task<bool> CreateFineAsync(FineCreateDTO dto, string policeId, string ip);

        Task<PaginationResult<FineResponseDTO>> GetMyFinesAsync(string userId, FineFilterDTO filter);
        Task<PaginationResult<FineResponseDTO>> GetFinesCreatedByPoliceAsync(string policeId, FineFilterDTO filter);
        Task<List<FineResponseDTO>> GetAllFinesAsync(int page, int pageSize);
        Task<object?> GetRecipientDetailsByPlateAsync(string plate);

    }

}
