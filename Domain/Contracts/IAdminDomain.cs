using DTO;
using DTO.UserDTO;
using Helpers.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IAdminDomain
    {
        Task<PaginationResult<UserDTO>> GetUsersPaginatedAsync(PaginationDTO dto);
        Task<bool> ChangeUserStatusAsync(string userId, string newStatus);

        Task<Dictionary<string, int>> GetUserCountAsync();
    }

}
