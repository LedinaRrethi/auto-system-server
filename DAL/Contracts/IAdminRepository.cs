using DTO;
using DTO.UserDTO;
using Entities.Models;
using Helpers.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Contracts
{
    public interface IAdminRepository
    {
        Task<PaginationResult<UserDTO>> GetAllUsersForApprovalAsync(PaginationDTO dto);
        void UpdateUser(Auto_Users user);

        Task<Auto_Users?> GetUserByIdAsync(string userId);
        Task<Dictionary<string, int>> CountUsersByStatusAsync();
    }
}
