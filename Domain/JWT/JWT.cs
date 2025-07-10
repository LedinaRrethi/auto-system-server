using Entities.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Domain.JWT;

public class JWT
{
    private readonly IConfiguration _config;
    public JWT(IConfiguration config)
    {
        _config = config;
    }

    public TokenData GenerateToken(Auto_Users user, IList<string> roles)
    {
        var jwtId = Guid.NewGuid().ToString();

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Jti, jwtId),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email ?? ""),
            new Claim("FullName", $"{user.FirstName} {user.LastName}")
        };

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["AppSettings:Token"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var expiry = DateTime.UtcNow.AddDays(7);

        var token = new JwtSecurityToken(
            issuer: _config["JWT:Issuer"],
            audience: _config["JWT:Audience"],
            claims: claims,
            expires: expiry,
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return new TokenData
        {
            Token = jwt,
            JwtId = jwtId,
            Expiry = expiry
        };
    }

    public RefreshToken GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);

        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomBytes),
            ExpiryDate = DateTime.UtcNow.AddDays(7),
            CreatedOn = DateTime.UtcNow
        };
    }
}
