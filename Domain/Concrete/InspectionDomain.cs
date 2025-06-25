using AutoMapper;
using DAL.Concrete;
using DAL.Contracts;
using DAL.UoW;
using Domain.Contracts;
using DTO;
using DTO.InspectionDTO;
using DTO.VehicleDTO;
using Entities.Models;
using Helpers.Pagination;
using Microsoft.AspNetCore.Http;

namespace Domain.Concrete
{
    public class InspectionDomain : DomainBase, IInspectionDomain
    {
        public InspectionDomain(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, mapper, httpContextAccessor) { }

        private IInspectionRepository _repo => _unitOfWork.GetRepository<IInspectionRepository>();
        private IRepository<Auto_InspectionDocs> _docRepo => _unitOfWork.GetRepository<IRepository<Auto_InspectionDocs>>();

        public async Task<PaginationResult<InspectionRequestListDTO>> GetMyRequestsAsync(string userId, PaginationDTO dto)
        {
            return await _repo.GetRequestsBySpecialistAsync(userId, dto);
        }

     

          
            public async Task<bool> ApproveInspectionAsync(InspectionApprovalDTO dto)
            {
                return await _repo.ApproveInspectionAsync(dto, GetCurrentUserId(), GetCurrentIp());
            }
    

     
        public async Task<List<VehicleDTO>> GetMyVehiclesAsync(string userId)
        {
            var vehicles = await _repo.GetVehiclesByUserIdAsync(userId);
            return _mapper.Map<List<VehicleDTO>>(vehicles);
        }

    }
}
