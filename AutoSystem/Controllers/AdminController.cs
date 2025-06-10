using Domain.Contracts;
using DTO.AdminnDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminDomain _domain;

        public AdminController(IAdminDomain domain)
        {
            _domain = domain;
        }

        [HttpGet("users")]
        public async Task<ActionResult<List<AdminDTO>>> GetUsers()
        {
            var users = await _domain.GetUsersAsync();
            return Ok(users);
        }

        [HttpPost("users/{userId}/status")]
        public async Task<ActionResult> UpdateStatus(string userId, [FromBody] string newStatus)
        {
            var result = await _domain.ChangeUserStatusAsync(userId, newStatus);
            if (!result) return BadRequest("Invalid status or user not found.");
            return Ok();
        }
    }
}
