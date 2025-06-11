using DTO.UserDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Contracts
{
    public interface IAdminRepository
    {
        Task<List<UserDTO>> GetAllUsersForApprovalAsync();
        Task<bool> UpdateUserStatusAsync(string userId, string newStatus);
    }
}
