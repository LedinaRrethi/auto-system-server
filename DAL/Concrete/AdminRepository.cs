﻿using DAL.Contracts;
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
            var users = await _context.Users
                .Include(u => u.Directorate)
                .Select(u => new
                {
                    u.Id,
                    u.FirstName,
                    u.FatherName,
                    u.LastName,
                    u.Email,
                    u.BirthDate,
                    u.Status,
                    u.CreatedOn,
                    u.SpecialistNumber,
                    DirectorateName = u.Directorate != null ? u.Directorate.DirectoryName : null
                })
                .ToListAsync();

            var result = new List<UserDTO>();

             foreach (var user in users)
            {
              
                var appUser = await _userManager.FindByIdAsync(user.Id);
                var roles = await _userManager.GetRolesAsync(appUser!);

                if (roles.Contains("Admin", StringComparer.OrdinalIgnoreCase))
                    continue;

                result.Add(new UserDTO
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    FatherName = user.FatherName,
                    LastName = user.LastName,
                    Email = user.Email!,
                    BirthDate = user.BirthDate,
                    Role = roles.FirstOrDefault() ?? "Unknown",
                    Status = user.Status.ToString(),
                    CreatedOn = user.CreatedOn,
                    SpecialistNumber = user.SpecialistNumber,
                    DirectorateName = user.DirectorateName
                });
            }

            var helper = new PaginationHelper<UserDTO>();

            return helper.GetPaginatedData(
                result,
                dto.Page,
                dto.PageSize,
                dto.SortField ?? "CreatedOn",
                dto.SortOrder ?? "desc",
                string.IsNullOrWhiteSpace(dto.Search)
                ? null
                : (Func<UserDTO, bool>)(u =>
                {
                    var fullName = $"{u.FirstName} {u.FatherName} {u.LastName}".Trim();
                    return fullName.Contains(dto.Search, StringComparison.OrdinalIgnoreCase)
                        || (!string.IsNullOrEmpty(u.Email) && u.Email.Contains(dto.Search, StringComparison.OrdinalIgnoreCase))
                        || (!string.IsNullOrEmpty(u.Role) && u.Role.Contains(dto.Search, StringComparison.OrdinalIgnoreCase))
                        || (!string.IsNullOrEmpty(u.Status) && u.Status.Contains(dto.Search, StringComparison.OrdinalIgnoreCase));
                })
            );
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
