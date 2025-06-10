using DTO.AdminnDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IAdminDomain
    {
        Task<List<AdminDTO>> GetUsersAsync();
        Task<bool> ChangeUserStatusAsync(string userId, string newStatus);
    }
}
