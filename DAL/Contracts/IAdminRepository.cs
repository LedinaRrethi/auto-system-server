using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.AdminnDTO;

namespace DAL.Contracts
{ 
        public interface IAdminRepository
        {
            Task<List<AdminDTO>> GetAllUsersForApprovalAsync();
            Task<bool> UpdateUserStatusAsync(string userId, string newStatus);
        }
}
