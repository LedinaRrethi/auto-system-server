using DAL.Contracts;
using DTO;
using DTO.InspectionDTO;
using Entities.Models;
using Helpers.Enumerations;
using Helpers.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Concrete
{
    public class InspectionRequestRepository : BaseRepository<Auto_InspectionRequests>, IInspectionRequestRepository
    {
        public InspectionRequestRepository(AutoSystemDbContext context) : base(context){}

        public async Task<int> CountInspectionsByDateAndDirectoryAsync(Guid directoryId, DateTime date)
        {
            return await _dbSet
                .CountAsync(r =>
                    r.IDFK_Directory == directoryId &&
                    r.RequestedDate.Date == date.Date &&
                    r.Invalidated == 0);
        }


        public async Task<bool> HasPendingRequestAsync(Guid vehicleId)
        {
            return await _dbSet
                .AnyAsync(r =>
                    r.IDFK_Vehicle == vehicleId &&
                    r.Status == InspectionStatus.Pending &&
                    r.Invalidated == 0);
        }

        public async Task<PaginationResult<MyInspectionRequestDTO>> GetCurrentUserPagedInspectionRequestsAsync(string userId, PaginationDTO dto)
        {
            var requestsQuery = _context.Auto_InspectionRequests
                .Include(r => r.Vehicle)
                .Include(r => r.Directory)
                .Where(r => r.CreatedBy == userId && r.Invalidated == 0 && r.Vehicle.Invalidated == 0)
                .OrderByDescending(r => r.RequestedDate)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(dto.Search))
            {
                var lower = dto.Search.ToLower();
                requestsQuery = requestsQuery.Where(r =>
                    r.Vehicle.PlateNumber.ToLower().Contains(lower) ||
                    r.Directory.DirectoryName.ToLower().Contains(lower));
            }

            var totalCount = await requestsQuery.CountAsync();

            var pageData = await requestsQuery
                .Skip((dto.Page - 1) * dto.PageSize)
                .Take(dto.PageSize)
                .ToListAsync();

            var requestIds = pageData.Select(r => r.IDPK_InspectionRequest).ToList();

            var inspections = await _context.Auto_Inspections
                .Where(i => requestIds.Contains(i.IDFK_InspectionRequest) && i.Invalidated == 0)
                .ToListAsync();

            var inspectionIds = inspections.Select(i => i.IDPK_Inspection).ToList();

            var docs = await _context.Auto_InspectionDocs
                .Where(d => inspectionIds.Contains(d.IDFK_Inspection) && d.Invalidated == 0)
                .ToListAsync();

            var result = pageData.Select(r =>
            {
                var inspection = inspections.FirstOrDefault(i => i.IDFK_InspectionRequest == r.IDPK_InspectionRequest);

                var inspectionDocs = inspection != null
                    ? docs.Where(d => d.IDFK_Inspection == inspection.IDPK_Inspection)
                        .Select(d => new InspectionDocumentDTO
                        {
                            IDPK_InspectionDoc = d.IDPK_InspectionDoc,
                            IDFK_InspectionRequest = r.IDPK_InspectionRequest,
                            DocumentName = d.DocumentName,
                        }).ToList()
                    : new List<InspectionDocumentDTO>();

                return new MyInspectionRequestDTO
                {
                    IDPK_InspectionRequest = r.IDPK_InspectionRequest,
                    PlateNumber = r.Vehicle.PlateNumber,
                    RequestedDate = r.RequestedDate.ToLocalTime(),
                    DirectorateName = r.Directory.DirectoryName,
                    Status = r.Status.ToString(),
                    Comment = inspection?.Comment,
                    Documents = inspectionDocs
                };
            }).ToList();

            return new PaginationResult<MyInspectionRequestDTO>
            {
                Items = result,
                Page = dto.Page,
                PageSize = dto.PageSize,
                HasNextPage = dto.Page * dto.PageSize < totalCount,
                Message = result.Any() ? "Success" : "No inspection request found."
            };
        }


        public async Task<string?> GetInspectionDocumentBase64Async(Guid documentId)
        {
            return await _context.Auto_InspectionDocs
                .Where(d => d.IDPK_InspectionDoc == documentId && d.Invalidated == 0)
                .Select(d => d.FileBase64)
                .FirstOrDefaultAsync();
        }


        public async Task<Dictionary<string, int>> CountInspectionsByStatusForSpecialistAsync(Guid directoryId)
        {
            var counts = await _context.Auto_InspectionRequests
                .Include(r=>r.Vehicle)
                .Where(r => r.IDFK_Directory == directoryId && r.Invalidated == 0 && r.Vehicle.Invalidated==0)
                .GroupBy(r => r.Status)
                .Select(g => new { Status = g.Key.ToString(), Count = g.Count() })
                .ToDictionaryAsync(g => g.Status, g => g.Count);

            return counts;
        }

        public async Task<Dictionary<string, int>> CountInspectionRequestsByUserAsync(string userId)
        {
            return await _context.Auto_InspectionRequests
                .Include(r=>r.Vehicle)
                .Where(r => r.CreatedBy == userId && r.Invalidated == 0 && r.Vehicle.Invalidated==0)
                .GroupBy(r => r.Status)
                .Select(g => new { Status = g.Key.ToString(), Count = g.Count() })
                .ToDictionaryAsync(g => g.Status, g => g.Count);
        }

    }
}