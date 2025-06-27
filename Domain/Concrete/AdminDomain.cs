using AutoMapper;
using DAL.Contracts;
using DAL.UoW;
using Domain.Contracts;
using DTO;
using DTO.UserDTO;
using Helpers.Pagination;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public class AdminDomain : DomainBase, IAdminDomain
    {

        public AdminDomain(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, mapper, httpContextAccessor)
        {}

        private IAdminRepository adminRepository => _unitOfWork.GetRepository<IAdminRepository>();

        public async Task<PaginationResult<UserDTO>> GetUsersPaginatedAsync(PaginationDTO dto)
        {
            return await adminRepository.GetAllUsersForApprovalAsync(dto);      
        }

        public Task<bool> ChangeUserStatusAsync(string userId, string newStatus)
        {
            return adminRepository.UpdateUserStatusAsync(userId, newStatus);
        }
    }
}
