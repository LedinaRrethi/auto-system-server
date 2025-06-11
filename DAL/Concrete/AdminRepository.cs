using DAL.Contracts;
using DTO.UserDTO;
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

        public async Task<List<UserDTO>> GetAllUsersForApprovalAsync()
        {
            var users = await _context.Users
                .Include(u => u.Directorate)
                .ToListAsync();

            var result = new List<UserDTO>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                result.Add(new UserDTO
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    FatherName = user.FatherName,
                    LastName = user.LastName,
                    Email = user.Email,
                    BirthDate = user.BirthDate,
                    Role = roles.FirstOrDefault() ?? "Unknown",
                    Status = user.Status.ToString(),
                    CreatedOn = user.CreatedOn,
                    SpecialistNumber = user.SpecialistNumber,
                    DirectorateName = user.Directorate?.DirectoryName
                });
            }

            return result;
        }

        public async Task<bool> UpdateUserStatusAsync(string userId, string newStatus)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return false;

            if (!Enum.TryParse(typeof(UserStatus), newStatus, true, out var statusEnum))
                return false;

            user.Status = (UserStatus)statusEnum!;
            user.ModifiedOn = DateTime.UtcNow;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
