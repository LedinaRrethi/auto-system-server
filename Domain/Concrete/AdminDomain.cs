using AutoMapper;
using DAL.Contracts;
using DAL.UoW;
using Domain.Contracts;
using DTO;
using DTO.UserDTO;
using Helpers.Enumerations;
using Helpers.Pagination;
using Microsoft.AspNetCore.Http;

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

        public async Task<bool> ChangeUserStatusAsync(string userId, string newStatus)
        {
            var user = await adminRepository.GetUserByIdAsync(userId);
            if (user == null)
                return false;

            if (!Enum.TryParse(typeof(UserStatus), newStatus, true, out var statusEnum))
                return false;

            user.Status = (UserStatus)statusEnum!;
            SetAuditOnUpdate(user);

            adminRepository.UpdateUser(user);
            await _unitOfWork.CommitAsync();

            return true;
        }


        public async Task<Dictionary<string, int>> GetUserCountAsync()
        {
            return await adminRepository.CountUsersByStatusAsync();
        }
    }
}
