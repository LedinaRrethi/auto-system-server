using DTO;
using DTO.InspectionDTO;
using Entities.Models;
using Helpers.Pagination;

namespace DAL.Contracts
{
    public interface IInspectionRepository : IRepository<Auto_Inspections>
    {
        Task<PaginationResult<InspectionRequestListDTO>> GetRequestsBySpecialistAsync(string specialistId, PaginationDTO dto);
        Task<List<Auto_Vehicles>> GetVehiclesByUserIdAsync(string userId);
        Task<Auto_Inspections?> GetInspectionWithRequestAsync(Guid inspectionId);

    }
}
