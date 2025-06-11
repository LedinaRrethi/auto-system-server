using AutoMapper;
using Domain.Contracts;
using DTO.UserDTO;
using Entities.Models;
using Helpers.Enumerations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

    public class AuthDomain : IAuthDomain
    {
        private readonly UserManager<Auto_Users> _userManager;
        private readonly SignInManager<Auto_Users> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AutoSystemDbContext _context;
        private readonly IMapper _mapper;
        private readonly JWT _jwt;

        public AuthDomain(
            UserManager<Auto_Users> userManager,
            SignInManager<Auto_Users> signInManager,
            RoleManager<IdentityRole> roleManager,
            AutoSystemDbContext context,
            IMapper mapper,
            IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
            _mapper = mapper;
            _jwt = new JWT(config);
        }

    public async Task RegisterAsync(RegisterDTO dto)
    {
        var existing = await _userManager.FindByEmailAsync(dto.Email);
        if (existing != null) throw new Exception("User already exists.");

        var user = new Auto_Users
        {
            FirstName = dto.FirstName,
            FatherName = dto.FatherName,
            LastName = dto.LastName,
            BirthDate = dto.BirthDate,
            Email = dto.Email,
            UserName = dto.Email,
            CreatedOn = DateTime.UtcNow,
            CreatedIp = "::1", 

            EmailConfirmed = true,
            Status = UserStatus.Pending,
            Invalidated = 0
        };

        // Nese eshte specialist
        if (dto.Role == "Specialist")
        {
            user.IsSpecialist = true;
            user.SpecialistNumber = dto.SpecialistNumber;
            user.IDFK_Directory = dto.DirectorateId;
        }

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            throw new Exception(string.Join("; ", result.Errors.Select(e => e.Description)));

        // Roli (ruhet nga ui)
        if (!await _roleManager.RoleExistsAsync(dto.Role))
            await _roleManager.CreateAsync(new IdentityRole(dto.Role));

        await _userManager.AddToRoleAsync(user, dto.Role);
    }

    public async Task<AuthResponseDTO> LoginAsync(LoginDTO dto, string ipAddress)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || user.Invalidated == 1)
                throw new Exception("Invalid credentials or blocked user.");      

            if (user.Status == UserStatus.Pending)
                throw new Exception("Your account is pending approval.");
            if (user.Status == UserStatus.Rejected)
                throw new Exception("Your account was rejected.");


            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!result.Succeeded) throw new Exception("Invalid credentials.");

            var roles = await _userManager.GetRolesAsync(user);

            var tokenData = _jwt.GenerateToken(user, roles);
            var refreshToken = _jwt.GenerateRefreshToken();

            var dbToken = new Auto_RefreshTokens
            {
                Token = refreshToken.Token,
                JwtId = tokenData.JwtId,
                IDFK_User = user.Id,
                CreatedAt = refreshToken.CreatedAt,
                ExpiryDate = refreshToken.ExpiryDate,
                CreatedByIp = ipAddress
            };

            await _context.Auto_RefreshTokens.AddAsync(dbToken);
            await _context.SaveChangesAsync();

            return new AuthResponseDTO
            {
                Token = tokenData.Token,
                ExpiresAt = tokenData.Expiry,
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<AuthResponseDTO> RefreshTokenAsync(string refreshToken, string ipAddress)
        {
            var token = await _context.Auto_RefreshTokens
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Token == refreshToken);

            if (token == null || token.IsRevoked || token.IsUsed || token.ExpiryDate < DateTime.UtcNow)
                throw new Exception("Invalid or expired refresh token.");

            token.IsUsed = true;
            token.IsRevoked = true;
            _context.Auto_RefreshTokens.Update(token);

            var user = token.User;
            var roles = await _userManager.GetRolesAsync(user);
            var tokenData = _jwt.GenerateToken(user, roles);
            var newRefresh = _jwt.GenerateRefreshToken();

            var dbNew = new Auto_RefreshTokens
            {
                Token = newRefresh.Token,
                JwtId = tokenData.JwtId,
                IDFK_User = user.Id,
                CreatedAt = newRefresh.CreatedAt,
                ExpiryDate = newRefresh.ExpiryDate,
                CreatedByIp = ipAddress
            };

            await _context.Auto_RefreshTokens.AddAsync(dbNew);
            await _context.SaveChangesAsync();

            return new AuthResponseDTO
            {
                Token = tokenData.Token,
                ExpiresAt = tokenData.Expiry,
                RefreshToken = newRefresh.Token
            };
        }

        public async Task LogoutAsync(string refreshToken)
        {
            var token = await _context.Auto_RefreshTokens
                //.AsTracking()
                .FirstOrDefaultAsync(t => t.Token == refreshToken);
            if (token == null)
            {
                Console.WriteLine("Token null ne LogoutAsync");
                return;
            }

            Console.WriteLine($"Revoking token: {token.Token}");
            /*
            token.IsRevoked = true;
            _context.Update(token);
            await _context.SaveChangesAsync();
            */
            token.IsRevoked = true;
            _context.Attach(token);
            _context.Entry(token).Property(t => t.IsRevoked).IsModified = true;
            await _context.SaveChangesAsync();


        }
    }
