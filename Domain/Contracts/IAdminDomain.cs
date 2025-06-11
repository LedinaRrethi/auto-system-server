using DTO.UserDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IAdminDomain
    {
        Task<PaginatedUserDTO> GetUsersPaginatedAsync(int page, int pageSize, string sortField, string sortOrder);
        Task<bool> ChangeUserStatusAsync(string userId, string newStatus);
    }

}
