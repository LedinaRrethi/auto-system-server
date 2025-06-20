using AutoMapper;
using DAL.Contracts;
using DAL.UoW;
using Domain.Contracts;
using DTO.InspectionDTO;
using Entities.Models;
using Helpers.Enumerations;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public class InspectionDomain : DomainBase, IInspectionDomain
    {
        public InspectionDomain(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor accessor)
            : base(unitOfWork, mapper, accessor) { }

        private IInspectionRepository _repo => _unitOfWork.GetRepository<IInspectionRepository>();

        public async Task<bool> CreateInspectionRequestAsync(InspectionRequestCreateDTO dto, string userId, string ip)
        {

            //kontroll nese ekziston nje kerkese pending per kete automjet
            bool hasPending = await _repo.HasPendingRequestAsync(dto.IDFK_Vehicle);
            if (hasPending)
                throw new InvalidOperationException("A pending inspection request already exists for this vehicle.");

            // 1 Nuk lejohen fundjavat
            var day = dto.RequestedDate.DayOfWeek;
            if (day == DayOfWeek.Saturday || day == DayOfWeek.Sunday)
                throw new InvalidOperationException("Inspections cannot be scheduled on weekends.");

            //2 Max 3 rezervime ne dite per nje drejtori
            int count = await _repo.CountInspectionsByDateAndDirectoryAsync(dto.IDFK_Directory, dto.RequestedDate);
            if (count >= 3)
                throw new InvalidOperationException("This directorate already has 3 inspections scheduled on this date.");

            // Krijimi i kerkeses
            var request = new Auto_InspectionRequests
            {
                IDPK_InspectionRequest = Guid.NewGuid(),
                IDFK_Vehicle = dto.IDFK_Vehicle,
                IDFK_Directory = dto.IDFK_Directory,
                RequestedDate = dto.RequestedDate,
                Status = InspectionStatus.Pending,
                CreatedBy = userId,
                CreatedIp = ip,
                CreatedOn = DateTime.UtcNow,
            };

            await _repo.AddInspectionRequestAsync(request);
            await _repo.SaveChangesAsync();
            return true;
        }

    }

}
