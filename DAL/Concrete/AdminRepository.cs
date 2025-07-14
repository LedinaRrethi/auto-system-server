using DAL.Contracts;
using DTO;
using DTO.UserDTO;
using Entities.Models;
using Helpers.Enumerations;
using Helpers.Pagination;
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


        public async Task<PaginationResult<UserDTO>> GetAllUsersForApprovalAsync(PaginationDTO dto)
        {
            var query = from user in _context.Users
                        join userRole in _context.UserRoles on user.Id equals userRole.UserId
                        join role in _context.Roles on userRole.RoleId equals role.Id
                        where role.Name.ToLower() != "admin"
                        select new UserDTO
                        {
                            Id = user.Id,
                            FirstName = user.FirstName,
                            FatherName = user.FatherName,
                            LastName = user.LastName,
                            Email = user.Email!,
                            BirthDate = user.BirthDate,
                            Role = role.Name,
                            Status = user.Status.ToString(),
                            CreatedOn = user.CreatedOn,
                            SpecialistNumber = user.SpecialistNumber,
                            DirectorateName = user.Directorate != null ? user.Directorate.DirectoryName : null
                        };

            if (!string.IsNullOrWhiteSpace(dto.Search))
            {
                var search = dto.Search.ToLower();
                query = query.Where(u =>
                    (u.FirstName + " " + u.FatherName + " " + u.LastName).ToLower().Contains(search) ||
                    u.Email.ToLower().Contains(search) ||
                    u.Role.ToLower().Contains(search) ||
                    u.Status.ToLower().Contains(search));
            }

            var totalCount = await query.CountAsync();

            if (!string.IsNullOrWhiteSpace(dto.SortField))
            {
                query = dto.SortOrder?.ToLower() == "desc"
                    ? query.OrderByDescending(e => EF.Property<object>(e, dto.SortField))
                    : query.OrderBy(e => EF.Property<object>(e, dto.SortField));
            }
            else
            {
                query = query.OrderByDescending(e => e.CreatedOn); // default
            }

            var users = await query
                .Skip((dto.Page - 1) * dto.PageSize)
                .Take(dto.PageSize)
                .ToListAsync();

            return new PaginationResult<UserDTO>
            {
                Items = users,
                Page = dto.Page,
                PageSize = dto.PageSize,
                HasNextPage = dto.Page * dto.PageSize < totalCount,
                Message = users.Any() ? "Success" : "No users found."
            };
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

        public async Task<Dictionary<string, int>> CountUsersByStatusAsync()
        {
            return await _context.Users
                .GroupBy(u => u.Status)
                .Select(g => new { Status = g.Key.ToString(), Count = g.Count() })
                .ToDictionaryAsync(g => g.Status, g => g.Count);
        }
    }
}
