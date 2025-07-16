using Domain.Concrete;
using Domain.Contracts;
using DTO;
using DTO.InspectionDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace AutoSystem.Controllers
{
  
    [ApiController]
    [Route("api/[controller]")]
    public class InspectionController : ControllerBase
    {
        private readonly IInspectionDomain _domain;

        public InspectionController(IInspectionDomain domain)
        {
            _domain = domain;
        }


        [HttpGet("my-requests")]
        [Authorize(Roles = "Specialist")]
        public async Task<IActionResult> GetRequests([FromQuery] PaginationDTO dto)
        {
            try{
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null) return Unauthorized();

                var result = await _domain.GetMyRequestsAsync(userId, dto);

                result.Message = result.Items.Count==0 ? "No inspection requests found." : "Success";

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "An error occurred while fetching inspections.",
                    details = ex.Message
                });
            }
        }

        [HttpPost("approve")]
        [Authorize(Roles = "Specialist")]
        public async Task<IActionResult> ApproveInspection([FromBody] InspectionApprovalDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            try
            {
                var result = await _domain.ApproveInspectionAsync(dto , userId , ip);
                return result ? Ok("Inspection updated with success.") : NotFound("Inspection not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "An error occurred while updating the inspection.",
                    details = ex.Message
                });
            }
        }

        [Authorize(Roles = "Individ")]
        [HttpGet("my-vehicles")]
        public async Task<IActionResult> GetMyVehicles()
        {
            try{
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                var vehicles = await _domain.GetMyVehiclesAsync(userId);
                return Ok(vehicles.Select(v => new
                {
                    id = v.IDPK_Vehicle,
                    plateNumber = v.PlateNumber
                }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "An error occurred while getting vehicles.",
                    details = ex.Message
                });
            }

        }

    }
}
