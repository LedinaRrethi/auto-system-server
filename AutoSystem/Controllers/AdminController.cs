using Domain.Contracts;
using DTO;
using DTO.UserDTO;
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
        public async Task<IActionResult> GetUsers([FromQuery] PaginationDTO dto)
        {
            var result = await _domain.GetUsersPaginatedAsync(dto);

            result.Message = !result.Items.Any() ? "No users found." : "Success";

            return Ok(result);
        }


        [HttpPost("users/{userId}/status")]
        public async Task<ActionResult> UpdateStatus(string userId, [FromBody] string newStatus)
        {
            var result = await _domain.ChangeUserStatusAsync(userId, newStatus);
            if (!result)
                return BadRequest("Invalid status or user not found.");
            return Ok(new { message = $"User status updated to {newStatus}" });
        }
    }

}
