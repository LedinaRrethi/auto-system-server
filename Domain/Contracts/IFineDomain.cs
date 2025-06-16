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

        Task<PaginationResult<FineResponseDTO>> GetMyFinesAsync(string userId, FineFilterDTO filter, int page, int pageSize);

        Task<PaginationResult<FineResponseDTO>> SearchFinesByPlateAsync(string plate, int page, int pageSize);

        Task<List<FineResponseDTO>> GetAllFinesAsync(int page, int pageSize);

        Task<VehicleOwnerInfoDTO?> GetVehicleOwnerInfoAsync(string plate);

    }

}
