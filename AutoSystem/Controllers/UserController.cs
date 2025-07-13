using Domain.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class ProfileController : ControllerBase
{
    private readonly IUserDomain _userDomain;

    public ProfileController(IUserDomain userDomain)
    {
        _userDomain = userDomain;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("User not authenticated");

        var profile = await _userDomain.GetUserProfileAsync(userId);
        if (profile == null)
            return NotFound("User not found");

        return Ok(profile);
    }
}
