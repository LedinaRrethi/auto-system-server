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
        private readonly AutoSystemDbContext _context;
        public InspectionRequestRepository(AutoSystemDbContext context) : base(context)
        {
            _context = context;
        }

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

        //public async Task<List<MyInspectionRequestDTO>> GetRequestsByUserAsync(string userId)
        //{
        //    var inspections = await _context.Auto_Inspections
        //        .Where(i => i.Invalidated == 0)
        //        .ToListAsync();

        //    var docs = await _context.Auto_InspectionDocs
        //        .Where(d => d.Invalidated == 0)
        //        .ToListAsync();

        //    var requests = await _context.Auto_InspectionRequests
        //        .Include(r => r.Vehicle)
        //        .Include(r => r.Directory)
        //        .Where(r => r.CreatedBy == userId && r.Invalidated == 0)
        //        .ToListAsync();

        //    var result = requests.Select(r =>
        //    {
        //        var inspection = inspections.FirstOrDefault(i => i.IDFK_InspectionRequest == r.IDPK_InspectionRequest);

        //        var inspectionDocs = docs
        //            .Where(d => d.IDFK_InspectionRequest == r.IDPK_InspectionRequest)
        //            .Select(d => new InspectionDocDTO
        //            {
        //                IDPK_InspectionDoc = d.IDPK_InspectionDoc,
        //                DocumentName = d.DocumentName,
        //                FileBase64 = d.FileBase64
        //            })
        //            .ToList();

        //        return new MyInspectionRequestDTO
        //        {
        //            IDPK_InspectionRequest = r.IDPK_InspectionRequest,
        //            PlateNumber = r.Vehicle.PlateNumber,
        //            RequestedDate = r.RequestedDate,
        //            DirectorateName = r.Directory.DirectoryName,
        //            Status = r.Status.ToString(),
        //            Comment = inspection?.Comment,
        //            Documents = inspectionDocs
        //        };
        //    }).ToList();

        //    return result;
        //}


        public async Task<PaginationResult<MyInspectionRequestDTO>> GetCurrentUserPagedInspectionRequestsAsync(string userId, PaginationDTO dto)
        {
            var inspections = await _context.Auto_Inspections
                .Where(i => i.Invalidated == 0)
                .ToListAsync();

            var docs = await _context.Auto_InspectionDocs
                .Where(d => d.Invalidated == 0)
                .ToListAsync();

            var requests = await _context.Auto_InspectionRequests
                .Include(r => r.Vehicle)
                .Include(r => r.Directory)
                .Where(r => r.CreatedBy == userId && r.Invalidated == 0)
                .ToListAsync();

            var result = requests.Select(r =>
            {
                var inspection = inspections.FirstOrDefault(i => i.IDFK_InspectionRequest == r.IDPK_InspectionRequest);
                var inspectionDocs = docs.Where(d => d.IDFK_Inspection == r.IDPK_InspectionRequest)
                    .Select(d => new InspectionDocumentDTO
                    {
                        IDPK_InspectionDoc = Guid.NewGuid(),
                        DocumentName = d.DocumentName,
                        FileBase64 = d.FileBase64
                    }).ToList();

                return new MyInspectionRequestDTO
                {
                    IDPK_InspectionRequest = r.IDPK_InspectionRequest,
                    PlateNumber = r.Vehicle.PlateNumber,
                    RequestedDate = r.RequestedDate,
                    DirectorateName = r.Directory.DirectoryName,
                    Status = r.Status.ToString(),
                    Comment = inspection?.Comment,
                    Documents = inspectionDocs.ToList()
                };
            });

            var helper = new PaginationHelper<MyInspectionRequestDTO>();
            Func<MyInspectionRequestDTO, bool> filter = null;

            if (!string.IsNullOrWhiteSpace(dto.Search))
            {
                var lower = dto.Search.ToLower();
                filter = x => x.PlateNumber.ToLower().Contains(lower)
                           || x.DirectorateName.ToLower().Contains(lower);
            }

            return helper.GetPaginatedData(result, dto.Page, dto.PageSize, dto.SortField, dto.SortOrder, filter);
        }



    }
}