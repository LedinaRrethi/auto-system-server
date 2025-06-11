using AutoMapper;
using DAL.Contracts;
using DAL.UoW;
using Domain.Contracts;
using DTO.UserDTO;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public class AdminDomain : DomainBase, IAdminDomain
    {
        private readonly PaginationHelper<UserDTO> _paginationHelper;

        public AdminDomain(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, mapper, httpContextAccessor)
        {
            _paginationHelper = new PaginationHelper<UserDTO>();
        }

        private IAdminRepository AdminRepository => _unitOfWork.GetRepository<IAdminRepository>();

        public async Task<PaginatedUserDTO> GetUsersPaginatedAsync(int page, int pageSize, string sortField, string sortOrder)
        {
            var users = await AdminRepository.GetAllUsersForApprovalAsync();
            var paginatedUsers = _paginationHelper.GetPaginatedData(users, page, pageSize, sortField, sortOrder).ToList();

            var totalPages = (int)Math.Ceiling((double)users.Count / pageSize);

            return new PaginatedUserDTO
            {
                Users = paginatedUsers,
                TotalPages = totalPages
            };
        }

        public Task<bool> ChangeUserStatusAsync(string userId, string newStatus)
        {
            return AdminRepository.UpdateUserStatusAsync(userId, newStatus);
        }
    }

}
