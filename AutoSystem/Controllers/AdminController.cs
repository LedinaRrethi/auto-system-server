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
            try
            {
                var result = await _domain.GetUsersPaginatedAsync(dto);

                result.Message = result.Items.Count == 0 ? "No users found." : "Success";

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "An error occurred while fetching users.",
                    details = ex.Message
                });
            }
        }

        [HttpPost("users/{userId}/status")]
        public async Task<ActionResult> UpdateStatus(string userId, [FromBody] string newStatus)
        {
            try
            {
                var result = await _domain.ChangeUserStatusAsync(userId, newStatus);

                if (!result)
                    return BadRequest(new { error = "Invalid status or user not found." });

                return Ok(new { message = $"User status updated to {newStatus}" });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "An error occurred while updating the user status.",
                    details = ex.Message
                });
            }
        }
    }

}
