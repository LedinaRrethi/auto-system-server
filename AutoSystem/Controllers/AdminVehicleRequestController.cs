using Domain.Contracts;
using DTO.VehicleRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminVehicleRequestController : ControllerBase
    {
        private readonly IAdminVehicleRequestDomain _domain;

        public AdminVehicleRequestController(IAdminVehicleRequestDomain domain)
        {
            _domain = domain;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _domain.GetAllRequestsAsync();
            return Ok(result);
        }

        [HttpPost("update-status/{requestId}")]
        public async Task<IActionResult> UpdateStatus(Guid requestId, [FromBody] VehicleChangeStatusDTO dto)
        {
            var success = await _domain.UpdateRequestStatusAsync(requestId, dto);
            if (!success)
                return BadRequest("Request not found or already handled.");

            return Ok(new { message = $"Request {dto.NewStatus}" });
        }
    }
}
