using Microsoft.AspNetCore.Http;
using Domain.Contracts;
using DTO.VehicleRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DTO.VehicleDTO;

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

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;


        [HttpPost("register")]
        public async Task<IActionResult> RegisterVehicle([FromBody] VehicleRegisterDTO dto)
        {
            var vehicleId = Guid.NewGuid(); // Generate a new Guid for the vehicleId
            await _domain.RegisterVehicleAsync(vehicleId, dto, GetUserId());
            return Ok(new { message = "Vehicle registration request submitted successfully." });
        }


        [HttpPut("request-update/{vehicleId}")]
        public async Task<IActionResult> RequestUpdate(Guid vehicleId, [FromBody] VehicleRegisterDTO dto)
        {
            await _domain.RequestVehicleUpdateAsync(vehicleId, dto, GetUserId());
            return Ok(new { message = "Vehicle update request submitted successfully." });
        }

        [HttpDelete("request-delete/{vehicleId}")]
        public async Task<IActionResult> RequestDelete(Guid vehicleId)
        {
            await _domain.RequestVehicleDeletionAsync(vehicleId, GetUserId());
            return Ok(new { message = "Delete request submitted successfully." });
        }


        [HttpGet("my-requests")]
        public async Task<IActionResult> MyRequests()
        {
            var requests = await _domain.GetMyRequestsAsync(GetUserId());
            return Ok(requests);
        }
    }
}
