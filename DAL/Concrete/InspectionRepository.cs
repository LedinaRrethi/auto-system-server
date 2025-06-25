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
                string.IsNullOrEmpty(dto.Search)
                    ? null
                    : i => i.PlateNumber.Contains(dto.Search, StringComparison.OrdinalIgnoreCase)
            );
        }


        public async Task<bool> ApproveInspectionAsync(InspectionApprovalDTO dto, string? userId, string ip)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var inspection = await _context.Set<Auto_Inspections>()
                    .Include(i => i.Request)
                    .FirstOrDefaultAsync(i => i.IDPK_Inspection == dto.IDPK_Inspection);

                if (inspection == null)
                    return false;

                foreach (var doc in dto.Documents)
                {
                    if (!doc.DocumentName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                        throw new Exception($"File '{doc.DocumentName}' is not pdf.");

                    var fileBytes = Convert.FromBase64String(doc.FileBase64);
                    if (fileBytes.Length > 5 * 1024 * 1024)
                        throw new Exception($"File '{doc.DocumentName}' is largen than 5MB.");
                }

                inspection.IsPassed = dto.IsPassed;
                inspection.Comment = dto.Comment;
                inspection.ModifiedBy = userId;
                inspection.ModifiedOn = DateTime.UtcNow;
                inspection.ModifiedIp = ip;

                inspection.Request.Status = dto.IsPassed ? InspectionStatus.Approved : InspectionStatus.Rejected;
                inspection.Request.ModifiedBy = userId;
                inspection.Request.ModifiedOn = DateTime.UtcNow;
                inspection.Request.ModifiedIp = ip;

                _context.Update(inspection);
                _context.Update(inspection.Request);

                foreach (var doc in dto.Documents)
                {
                    var docEntity = new Auto_InspectionDocs
                    {
                        IDPK_InspectionDoc = Guid.NewGuid(),
                        IDFK_Inspection = dto.IDPK_Inspection,
                        DocumentName = doc.DocumentName,
                        FileBase64 = doc.FileBase64,
                        CreatedBy = userId!,
                        CreatedOn = DateTime.UtcNow,
                        CreatedIp = ip
                    };

                    _context.Add(docEntity);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }



        public Task<List<Auto_Vehicles>> GetVehiclesByUserIdAsync(string userId)
        {
            return _context.Auto_Vehicles
                .Where(v => v.IDFK_Owner == userId && v.Invalidated == 0 && v.Status == VehicleStatus.Approved)
                .ToListAsync();
        }

    }
}
