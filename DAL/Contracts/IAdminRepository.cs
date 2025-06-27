using DTO;
using DTO.UserDTO;
using Helpers.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Contracts
{
    public interface IAdminRepository
    {
        Task<PaginationResult<UserDTO>> GetAllUsersForApprovalAsync(PaginationDTO dto);
        Task<bool> UpdateUserStatusAsync(string userId, string newStatus);
    }
}
