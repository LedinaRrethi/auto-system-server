using DAL.Contracts;
using DTO.InspectionDTO;
using Entities.Models;
using Helpers.Enumerations;
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

        //public async Task<Auto_InspectionRequests?> GetRequestByIdAsync(Guid requestId)
        //{
        //    return await _context.Auto_InspectionRequests
        //        .FirstOrDefaultAsync(x => x.IDPK_InspectionRequest == requestId && x.Invalidated == 0);
        //}

        public async Task<List<InspectionRequestListDTO>> GetRequestsBySpecialistAsync(string specialistId)
        {
            var specialistDirectoryId = await _context.Users
                .OfType<Auto_Users>()
                .Where(u => u.Id == specialistId && u.IDFK_Directory != null)
                .Select(u => u.IDFK_Directory)
                .FirstOrDefaultAsync();

            if (specialistDirectoryId == null)
                return new();

            var inspections = await _context.Auto_Inspections
                .Include(i => i.Request)
                    .ThenInclude(r => r.Vehicle)
                .Include(i => i.InspectionDocs)
                .Where(i => i.Request.IDFK_Directory == specialistDirectoryId && i.Invalidated == 0)
                .ToListAsync();

            return inspections.Select(i => new InspectionRequestListDTO
            {
                IDPK_InspectionRequest = i.IDFK_InspectionRequest,
                PlateNumber = i.Request.Vehicle.PlateNumber,
                RequestedDate = i.Request.RequestedDate,
                Status = i.Request.Status.ToString(),
                Comment = i.Comment,
                IsPassed = i.IsPassed,
                Documents = i.InspectionDocs.Select(d => new InspectionDocumentDTO
                {
                    IDPK_InspectionDoc = d.IDPK_InspectionDoc,
                    IDFK_InspectionRequest = d.IDFK_Inspection,
                    DocumentName = d.DocumentName,
                    FileBase64 = d.FileBase64
                }).ToList()
            }).ToList();
        }



        public Task<List<Auto_Vehicles>> GetVehiclesByUserIdAsync(string userId)
        {
            return _context.Auto_Vehicles
                .Where(v => v.IDFK_Owner == userId && v.Invalidated == 0 && v.Status == VehicleStatus.Approved)
                .ToListAsync();
        }

    }
}
