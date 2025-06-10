using AutoMapper;
using DAL.Contracts;
using DAL.UoW;
using Domain.Contracts;
using DTO.AdminnDTO;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public class AdminDomain : DomainBase, IAdminDomain
    {

        public AdminDomain(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, mapper, httpContextAccessor) { }

        private IAdminRepository AdminRepository => _unitOfWork.GetRepository<IAdminRepository>();


        public Task<List<AdminDTO>> GetUsersAsync()
        {
            return AdminRepository.GetAllUsersForApprovalAsync();
        }

        public Task<bool> ChangeUserStatusAsync(string userId, string newStatus)
        {
            return AdminRepository.UpdateUserStatusAsync(userId, newStatus);
        }
    }
}
