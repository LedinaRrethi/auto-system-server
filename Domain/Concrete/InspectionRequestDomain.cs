using AutoMapper;
using DAL.Concrete;
using DAL.Contracts;
using DAL.UoW;
using Domain.Contracts;
using DTO;
using DTO.InspectionDTO;
using Entities.Models;
using Helpers.Enumerations;
using Helpers.Pagination;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public class InspectionRequestDomain : DomainBase, IInspectionRequestDomain
    {
        private readonly IInspectionRequestRepository _repo;

        private IRepository<Auto_Inspections> _inspectionRepo => _unitOfWork.GetRepository<IRepository<Auto_Inspections>>();


        public InspectionRequestDomain(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor accessor)
            : base(unitOfWork, mapper, accessor) {
            _repo = unitOfWork.GetRepository<IInspectionRequestRepository>();

        }

        public async Task<bool> CreateInspectionRequestAsync(InspectionRequestCreateDTO dto)
        {
            if (await _repo.HasPendingRequestAsync(dto.IDFK_Vehicle))
                throw new InvalidOperationException("A pending inspection request already exists for this vehicle.");

            var requestedDateUtc = DateTime.SpecifyKind(dto.RequestedDate, DateTimeKind.Utc);

            var requestedDateLocal = requestedDateUtc.ToLocalTime();
            if (requestedDateLocal.DayOfWeek == DayOfWeek.Saturday || requestedDateLocal.DayOfWeek == DayOfWeek.Sunday)
                throw new InvalidOperationException("Inspections cannot be scheduled on weekends.");

            int count = await _repo.CountInspectionsByDateAndDirectoryAsync(dto.IDFK_Directory, requestedDateUtc);
            if (count >= 3)
                throw new InvalidOperationException("This directorate already has 3 inspections scheduled on this date.");

            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var request = _mapper.Map<Auto_InspectionRequests>(dto);
                request.IDPK_InspectionRequest = Guid.NewGuid();
                request.RequestedDate = requestedDateUtc; 

                SetAuditOnCreate(request);

                await _repo.AddAsync(request);

                var inspection = new Auto_Inspections
                {
                    IDPK_Inspection = Guid.NewGuid(),
                    IDFK_InspectionRequest = request.IDPK_InspectionRequest,
                    Invalidated = 0
                };
                SetAuditOnCreate(inspection);



                await _inspectionRepo.AddAsync(inspection);

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

        public async Task<PaginationResult<MyInspectionRequestDTO>> GetCurrentUserPagedInspectionRequestsAsync(PaginationDTO dto)
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId)) 
                return new PaginationResult<MyInspectionRequestDTO>();
            return await _repo.GetCurrentUserPagedInspectionRequestsAsync(userId, dto);
        }
    }
}
