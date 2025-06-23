using AutoMapper;
using DAL.UoW;
using Domain.Concrete;
using Domain.Contracts;
using DTO.UserDTO;
using Entities.Models;
using Helpers.Enumerations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class AuthDomain : DomainBase, IAuthDomain
{
    private readonly UserManager<Auto_Users> _userManager;
    private readonly SignInManager<Auto_Users> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly AutoSystemDbContext _context;
    private readonly JWT _jwt;

    public AuthDomain(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        UserManager<Auto_Users> userManager,
        SignInManager<Auto_Users> signInManager,
        RoleManager<IdentityRole> roleManager,
        AutoSystemDbContext context,
        IConfiguration config)
        : base(unitOfWork, mapper, httpContextAccessor)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _context = context;
        _jwt = new JWT(config);
    }

    public async Task RegisterAsync(RegisterDTO dto)
    {
        var existing = await _userManager.FindByEmailAsync(dto.Email);
        if (existing != null)
            throw new Exception("A user with this email already exists.");


        if (dto.Role == UserRole.Specialist)
        {
            if (string.IsNullOrWhiteSpace(dto.SpecialistNumber) || dto.DirectorateId == null)
                throw new Exception("Specialists must provide both a specialist number and a directorate.");
        }

        var user = _mapper.Map<Auto_Users>(dto);

        SetAuditOnCreate(user);

        user.UserName = dto.Email;
        user.EmailConfirmed = true;
        user.Status = UserStatus.Pending;
        user.Invalidated = 0;

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new Exception($"Error creating user: {errorMessage}");
        }

        var roleName = dto.Role.ToString();
        if (!await _roleManager.RoleExistsAsync(roleName))
            await _roleManager.CreateAsync(new IdentityRole(roleName));

        await _userManager.AddToRoleAsync(user, roleName);
    }

    public async Task<AuthResponseDTO> LoginAsync(LoginDTO dto, string ipAddress)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            throw new Exception("Invalid email or password.");

        if (user.Status == UserStatus.Pending)
            throw new Exception("Your account is pending approval.");

        if (user.Status == UserStatus.Rejected)
            throw new Exception("Your account was rejected.");

        var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
        if (!result.Succeeded)
            throw new Exception("Invalid credentials.");

        var roles = await _userManager.GetRolesAsync(user);
        var tokenData = _jwt.GenerateToken(user, roles);
        var refreshToken = _jwt.GenerateRefreshToken();

        var dbToken = new Auto_RefreshTokens
        {
            Token = refreshToken.Token,
            JwtId = tokenData.JwtId,
            IDFK_User = user.Id,
            CreatedOn = refreshToken.CreatedOn,
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
            CreatedOn = newRefresh.CreatedOn,
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
            .FirstOrDefaultAsync(t => t.Token == refreshToken);

        if (token == null) return;

        token.IsRevoked = true;
        _context.Attach(token);
        _context.Entry(token).Property(t => t.IsRevoked).IsModified = true;

        await _context.SaveChangesAsync();
    }
}
