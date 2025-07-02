using DAL.Contracts;
using DTO;
using DTO.InspectionDTO;
using Entities.Models;
using Helpers.Enumerations;
using Helpers.Pagination;
using Microsoft.EntityFrameworkCore;

namespace DAL.Concrete
{
    public class InspectionRepository : BaseRepository<Auto_Inspections>, IInspectionRepository
    {
        private readonly AutoSystemDbContext _context;

        public InspectionRepository(AutoSystemDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PaginationResult<InspectionRequestListDTO>> GetRequestsBySpecialistAsync(string specialistId, PaginationDTO dto)
        {
            var specialistDirectoryId = await _context.Users
                .OfType<Auto_Users>()
                .Where(u => u.Id == specialistId && u.IDFK_Directory != null)
                .Select(u => u.IDFK_Directory)
                .FirstOrDefaultAsync();

            if (specialistDirectoryId == null)
                return new PaginationResult<InspectionRequestListDTO>();

            var inspections = await _context.Auto_Inspections
                .Include(i => i.Request)
                    .ThenInclude(r => r.Vehicle)
                .Include(i => i.InspectionDocs)
                .Where(i => i.Request.IDFK_Directory == specialistDirectoryId && i.Invalidated == 0)
                .ToListAsync();

            var projected = inspections.Select(i => new InspectionRequestListDTO
            {
                IDPK_Inspection = i.IDPK_Inspection,
                IDPK_InspectionRequest = i.IDFK_InspectionRequest,
                PlateNumber = i.Request.Vehicle.PlateNumber,
                RequestedDate = i.Request.RequestedDate,
                Status = i.Request.Status.ToString(),
                Comment = i.Comment,
                IsPassed = i.IsPassed,
                Documents = i.InspectionDocs.Select(d => new InspectionDocumentDTO
                {
                    IDPK_InspectionDoc = Guid.NewGuid(),
                    IDFK_InspectionRequest = d.IDFK_Inspection,
                    DocumentName = d.DocumentName,
                    FileBase64 = d.FileBase64
                }).ToList()
            });

            var helper = new PaginationHelper<InspectionRequestListDTO>();
            return helper.GetPaginatedData(
                projected,
                dto.Page,
                dto.PageSize,
                dto.SortField,
                dto.SortOrder,
                string.IsNullOrEmpty(dto.Search) ? null: i => i.PlateNumber.Contains(dto.Search, StringComparison.OrdinalIgnoreCase)
                 || i.Status.Contains(dto.Search, StringComparison.OrdinalIgnoreCase)
            );
        }

        public async Task<Auto_Inspections?> GetInspectionWithRequestAsync(Guid inspectionId)
        {
            return await _context.Auto_Inspections
                .Include(i => i.Request)
                .ThenInclude(r => r.Vehicle)
                .FirstOrDefaultAsync(i => i.IDPK_Inspection == inspectionId);
        }

        public Task<List<Auto_Vehicles>> GetVehiclesByUserIdAsync(string userId)
        {
            return _context.Auto_Vehicles
                .Where(v => v.IDFK_Owner == userId && v.Invalidated == 0 && v.Status == VehicleStatus.Approved)
                .ToListAsync();
        }

        public Task SaveChangesAsync() => _context.SaveChangesAsync();

    }
}
