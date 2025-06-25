using Domain.Concrete;
using Domain.Contracts;
using DTO;
using DTO.InspectionDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var result = await _domain.GetMyRequestsAsync(userId, dto);
            return Ok(result);
        }

        [HttpPost("approve")]
        [Authorize(Roles = "Specialist")]
        public async Task<IActionResult> ApproveInspection([FromBody] InspectionApprovalDTO dto)
        {
            try
            {
                var result = await _domain.ApproveInspectionAsync(dto);
                return result ? Ok("Inspektimi u përditësua me sukses.") : NotFound("Inspektimi nuk u gjet.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Individ")]
        [HttpGet("my-vehicles")]
        public async Task<IActionResult> GetMyVehicles()
        {
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

    }
}
