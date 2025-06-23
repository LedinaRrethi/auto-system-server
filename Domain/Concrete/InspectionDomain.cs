using AutoMapper;
using DAL.Contracts;
using DAL.UoW;
using Domain.Contracts;
using DTO.InspectionDTO;
using Entities.Models;
using Microsoft.AspNetCore.Http;

namespace Domain.Concrete
{
    public class InspectionDomain : DomainBase, IInspectionDomain
    {
        public InspectionDomain(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, mapper, httpContextAccessor) { }

        private IInspectionRepository _repo => _unitOfWork.GetRepository<IInspectionRepository>();
        private IRepository<Auto_InspectionDocs> _docRepo => _unitOfWork.GetRepository<IRepository<Auto_InspectionDocs>>();

        public async Task<List<InspectionRequestCreateDTO>> GetMyRequestsAsync()
        {
            var userId = GetCurrentUserId();
            var list = await _repo.GetRequestsBySpecialistAsync(userId);
            return _mapper.Map<List<InspectionRequestCreateDTO>>(list);
        }

        public async Task<bool> ReviewInspectionAsync(InspectionReviewDTO dto)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync();

            var request = await _repo.GetRequestByIdAsync(dto.IDFK_InspectionRequest);
            if (request == null || request.Status != Helpers.Enumerations.InspectionStatus.Pending)
                return false;

            var inspection = _mapper.Map<Auto_Inspections>(dto);
            inspection.IDPK_Inspection = Guid.NewGuid();
            inspection.IDFK_Specialist = GetCurrentUserId();
            SetAuditOnCreate(inspection);

            request.Status = dto.IsPassed
                ? Helpers.Enumerations.InspectionStatus.Approved
                : Helpers.Enumerations.InspectionStatus.Rejected;
            SetAuditOnUpdate(request);

            try
            {
                await _repo.AddAsync(inspection);
                await _repo.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> UploadDocumentsAsync(List<InspectionDocumentUploadDTO> documents)
        {
            if (!documents.Any()) return false;

            var mapped = documents.Select(doc =>
            {
                var entity = _mapper.Map<Auto_InspectionDocs>(doc);
                entity.IDPK_InspectionDoc = Guid.NewGuid();
                SetAuditOnCreate(entity);
                return entity;
            }).ToList();

            await _docRepo.AddRangeAsync(mapped);
            await _docRepo.SaveChangesAsync();
            return true;
        }

        public async Task<List<InspectionDocDTO>> GetDocumentsAsync(Guid requestId)
        {
            var all = await _docRepo.FindAsync(d => d.IDFK_InspectionRequest == requestId && d.Invalidated == 0);
            return _mapper.Map<List<InspectionDocDTO>>(all);
        }

        public async Task<bool> DeleteDocumentAsync(Guid docId)
        {
            var doc = await _docRepo.GetByIdAsync(docId);
            if (doc == null) return false;

            doc.Invalidated = 1;
            SetAuditOnUpdate(doc);
            await _docRepo.UpdateAsync(doc);
            await _docRepo.SaveChangesAsync();
            return true;
        }
    }
}
