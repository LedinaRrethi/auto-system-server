using Domain.Contracts;
using DTO.UserDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    [AllowAnonymous] //TODO
    public class AdminController : ControllerBase
    {
        private readonly IAdminDomain _domain;

        public AdminController(IAdminDomain domain)
        {
            _domain = domain;
        }

        [HttpGet("users")]
        public async Task<ActionResult<PaginatedUserDTO>> GetUsers(
            int page = 1,
            int pageSize = 10,
            string sortField = "CreatedOn",
            string sortOrder = "desc")
        {
            var result = await _domain.GetUsersPaginatedAsync(page, pageSize, sortField, sortOrder);
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
