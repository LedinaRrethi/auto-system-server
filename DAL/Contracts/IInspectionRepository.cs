using DTO;
using DTO.InspectionDTO;
using Entities.Models;
using Helpers.Pagination;

namespace DAL.Contracts
{
    public interface IInspectionRepository : IRepository<Auto_Inspections>
    {
        //Task<Auto_InspectionRequests?> GetRequestByIdAsync(Guid requestId);

        Task<PaginationResult<InspectionRequestListDTO>> GetRequestsBySpecialistAsync(string specialistId, PaginationDTO dto);


        Task<List<Auto_Vehicles>> GetVehiclesByUserIdAsync(string userId);

    }
}
