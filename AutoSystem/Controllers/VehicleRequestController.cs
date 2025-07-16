using Microsoft.AspNetCore.Http;
using Domain.Contracts;
using DTO.VehicleRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DTO.VehicleDTO;
using DTO;

namespace AutoSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Individ")]
    public class VehicleRequestController : ControllerBase
    {
        private readonly IVehicleRequestDomain _domain;

        public VehicleRequestController(IVehicleRequestDomain domain)
        {
            _domain = domain;
        }

        private string GetUserId()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(id))
                throw new Exception("User ID not found in claims.");
            return id;
        }


        [HttpPost("register")]
        public async Task<IActionResult> RegisterVehicle([FromBody] VehicleRegisterDTO dto)
        {
            try
            {
                var vehicleId = Guid.NewGuid();
                await _domain.RegisterVehicleAsync(vehicleId, dto, GetUserId());
                return Ok(new { message = "Vehicle registration request submitted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("request-update/{vehicleId}")]
        public async Task<IActionResult> RequestUpdate(Guid vehicleId, [FromBody] VehicleRegisterDTO dto)
        {
            try
            {
                await _domain.RequestVehicleUpdateAsync(vehicleId, dto, GetUserId());
                return Ok(new { message = "Vehicle update request submitted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("request-delete/{vehicleId}")]
        public async Task<IActionResult> RequestDelete(Guid vehicleId)
        {
            try
            {
                await _domain.RequestVehicleDeletionAsync(vehicleId, GetUserId());
                return Ok(new { message = "Vehicle delete request submitted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpGet("my-requests")]
        public async Task<IActionResult> MyRequests([FromQuery] PaginationDTO dto)
        {
            try
            {
                var requests = await _domain.GetMyRequestsAsync(GetUserId(), dto);

                requests.Message = !requests.Items.Any() ? "No vehicles found." : "Success";

                return Ok(requests);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpGet("by-id/{vehicleId}")]
        public async Task<IActionResult> GetVehicleById(Guid vehicleId)
        {
            try
            {
                var vehicle = await _domain.GetVehicleForEditAsync(vehicleId, GetUserId());
                return Ok(vehicle);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}
