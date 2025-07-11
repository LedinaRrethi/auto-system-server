using Domain.Contracts;
using DTO;
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
        public async Task<IActionResult> GetAll([FromQuery] PaginationDTO dto)
        {
            var result = await _domain.GetAllRequestsAsync(dto);

            result.Message = !result.Items.Any() ? "No vehicles to display." : "Success";

            return Ok(result);  
        }


        [HttpPost("update-status/{requestId}")]
        public async Task<IActionResult> UpdateStatus(Guid requestId, [FromBody] VehicleChangeStatusDTO dto)
        {
            try
            {
                var success = await _domain.UpdateRequestStatusAsync(requestId, dto);
                if (!success)
                    return BadRequest(new { error = "Request not found or already handled." });

                return Ok(new { message = $"Request status updated to {dto.NewStatus}" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}
