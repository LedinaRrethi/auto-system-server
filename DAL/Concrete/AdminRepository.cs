using DAL.Contracts;
using DTO.AdminnDTO;
using Entities.Models;
using Helpers.Enumerations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AutoSystemDbContext _context;
        private readonly UserManager<Auto_Users> _userManager;

        public AdminRepository(AutoSystemDbContext context, UserManager<Auto_Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<AdminDTO>> GetAllUsersForApprovalAsync()
        {
            var users = await _context.Users.ToListAsync();

            var result = new List<AdminDTO>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                result.Add(new AdminDTO
                {
                    Id = user.Id,
                    Name = $"{user.FirstName} {user.LastName}",
                    Email = user.Email,
                    Role = roles.FirstOrDefault() ?? "Unknown",
                    Status = user.Status.ToString(),
                    CreatedAt = user.CreatedOn
                });
            }

            return result;
        }

        public async Task<bool> UpdateUserStatusAsync(string userId, string newStatus)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return false;

            if (Enum.TryParse(typeof(UserStatus), newStatus, out var statusEnum))
            {
                user.Status = (UserStatus)statusEnum!;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
