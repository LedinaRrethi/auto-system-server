using AutoMapper;
using DAL.Contracts;
using DAL.UoW;
using Domain.Contracts;
using DTO.InspectionDTO;
using Entities.Models;
using Helpers.Enumerations;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public class InspectionRequestDomain : DomainBase, IInspectionRequestDomain
    {
        public InspectionRequestDomain(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor accessor)
            : base(unitOfWork, mapper, accessor) { }

        private IInspectionRequestRepository _repo => _unitOfWork.GetRepository<IInspectionRequestRepository>();

        public async Task<bool> CreateInspectionRequestAsync(InspectionRequestCreateDTO dto)
        {
            if (await _repo.HasPendingRequestAsync(dto.IDFK_Vehicle))
                throw new InvalidOperationException("A pending inspection request already exists for this vehicle.");

            if (dto.RequestedDate.DayOfWeek == DayOfWeek.Saturday || dto.RequestedDate.DayOfWeek == DayOfWeek.Sunday)
                throw new InvalidOperationException("Inspections cannot be scheduled on weekends.");

            int count = await _repo.CountInspectionsByDateAndDirectoryAsync(dto.IDFK_Directory, dto.RequestedDate);
            if (count >= 3)
                throw new InvalidOperationException("This directorate already has 3 inspections scheduled on this date.");

            var request = _mapper.Map<Auto_InspectionRequests>(dto);
            request.IDPK_InspectionRequest = Guid.NewGuid();
            SetAuditOnCreate(request);

            await _repo.AddAsync(request);  
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
