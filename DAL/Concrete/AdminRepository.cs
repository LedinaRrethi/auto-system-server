using DAL.Contracts;
using DTO;
using DTO.UserDTO;
using Entities.Models;
using Helpers.Enumerations;
using Helpers.Pagination;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AutoSystemDbContext _context;
        
        public AdminRepository(AutoSystemDbContext context)
        {
            _context = context;
        }

        public async Task<PaginationResult<UserDTO>> GetAllUsersForApprovalAsync(PaginationDTO dto)
        {
            var query = from user in _context.Users.AsNoTracking()
                        join userRole in _context.UserRoles on user.Id equals userRole.UserId
                        join role in _context.Roles on userRole.RoleId equals role.Id
                        where (role.Name ?? "").ToLower() != "admin"
                        select new
                        {
                            user.Id,
                            user.FirstName,
                            user.FatherName,
                            user.LastName,
                            user.Email,
                            user.BirthDate,
                            user.Status,
                            user.CreatedOn,
                            user.SpecialistNumber,
                            DirectorateName = user.Directorate != null ? user.Directorate.DirectoryName : null,
                            RoleName = role.Name
                        };

            if (!string.IsNullOrWhiteSpace(dto.Search))
            {
                var search = dto.Search.ToLower();

                var statusMatch = Enum.GetValues(typeof(UserStatus))
                    .Cast<UserStatus>()
                    .FirstOrDefault(s => s.ToString().ToLower().Contains(search));

                bool isStatusSearch = statusMatch.ToString().ToLower().Contains(search);

                if (isStatusSearch)
                {
                    int statusValue = (int)statusMatch;
                    query = query.Where(u => (int)u.Status == statusValue);
                }
                else
                {
                    query = query.Where(u =>
                        (u.FirstName + " " + u.FatherName + " " + u.LastName).ToLower().Contains(search) ||
                        u.Email.ToLower().Contains(search) ||
                        u.RoleName.ToLower().Contains(search)
                    );
                }
            }

            //var totalCount = await query.CountAsync();

            if (!string.IsNullOrWhiteSpace(dto.SortField))
            {
                query = dto.SortOrder?.ToLower() == "desc"
                    ? query.OrderByDescending(e => EF.Property<object>(e, dto.SortField))
                    : query.OrderBy(e => EF.Property<object>(e, dto.SortField));
            }
            else
            {
                query = query.OrderByDescending(e => e.CreatedOn);
            }

            var usersRaw = await query
                .Skip((dto.Page - 1) * dto.PageSize)
                .Take(dto.PageSize + 1)
                .ToListAsync();

            var hasNextPage = usersRaw.Count > dto.PageSize;
            var pageUsers = usersRaw.Take(dto.PageSize).ToList();

            var users = usersRaw.Select(u => new UserDTO
            {
                Id = u.Id,
                FirstName = u.FirstName,
                FatherName = u.FatherName,
                LastName = u.LastName,
                Email = u.Email!,
                BirthDate = u.BirthDate,
                Role = u.RoleName,
                Status = u.Status.ToString(),
                CreatedOn = u.CreatedOn,
                SpecialistNumber = u.SpecialistNumber,
                DirectorateName = u.DirectorateName
            }).ToList();

            return new PaginationResult<UserDTO>
            {
                Items = users,
                Page = dto.Page,
                PageSize = dto.PageSize,
                HasNextPage = hasNextPage,
                Message = users.Any() ? "Success" : "No users found."
            };
        }

        public async Task<Auto_Users?> GetUserByIdAsync(string userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }



        public void UpdateUser(Auto_Users user)
        {
            _context.Users.Update(user);
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
