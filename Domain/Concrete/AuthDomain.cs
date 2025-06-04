using AutoMapper;
using Domain.JWT;
using DTO.UserDTO;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

public class AuthDomain : IAuthDomain
{
    private readonly UserManager<Auto_Users> _userManager;
    private readonly SignInManager<Auto_Users> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMapper _mapper;
    private readonly JWT _jwt;

    public AuthDomain(
        UserManager<Auto_Users> userManager,
        SignInManager<Auto_Users> signInManager,
        RoleManager<IdentityRole> roleManager,
        IMapper mapper,
        IConfiguration config)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _mapper = mapper;
        _jwt = new JWT(config);
    }

    public async Task RegisterAsync(RegisterDto dto)
    {
        var existing = await _userManager.FindByEmailAsync(dto.Email);
        if (existing != null)
            throw new Exception("User already exists");

        var user = _mapper.Map<Auto_Users>(dto);
        user.UserName = dto.Email;
        user.EmailConfirmed = true;

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            throw new Exception(string.Join("; ", result.Errors.Select(e => e.Description)));

        if (!await _roleManager.RoleExistsAsync("Individ"))
            await _roleManager.CreateAsync(new IdentityRole("Individ"));

        await _userManager.AddToRoleAsync(user, "Individ");
    }

    public async Task<string> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null || user.Invalidated == 1)
            throw new Exception("Invalid credentials");

        var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
        if (!result.Succeeded)
            throw new Exception("Invalid credentials");

        var roles = await _userManager.GetRolesAsync(user);
        return _jwt.CreateToken(user, roles);
    }
}
