using Domain.Contracts;
using DTO;
using DTO.FineDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AutoSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FineController : ControllerBase
    {
        private readonly IFineDomain _domain;

        public FineController(IFineDomain domain)
        {
            _domain = domain;
        }

        [Authorize(Roles = "Police")]
        [HttpPost]
        public async Task<IActionResult> AddFine([FromBody] FineCreateDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            var success = await _domain.CreateFineAsync(dto, userId, ip);
            if (success)
                return Ok("Fine successfully issued.");
            return BadRequest("An error occurred while registering the fine.");
        }

        [Authorize(Roles = "Individ")]
        [HttpGet("my-fines")]
        public async Task<IActionResult> GetMyFines([FromQuery] FineFilterDTO filter)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _domain.GetMyFinesAsync(userId, filter);
            return Ok(result);
        }

        [Authorize(Roles = "Police")]
        [HttpGet("my-issued-fines")]
        public async Task<IActionResult> GetPoliceFines([FromQuery] FineFilterDTO filter)
        {
            var policeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _domain.GetFinesCreatedByPoliceAsync(policeId, filter);
            return Ok(result);
        }

        [Authorize(Roles = "Police")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllFines([FromQuery] FineFilterDTO filter)
        {
            var result = await _domain.GetAllFinesAsync(filter);
            return Ok(result);
        }

        [Authorize(Roles = "Police")]
        [HttpGet("recipient-details")]
        public async Task<IActionResult> GetRecipientDetailsByPlate([FromQuery] string plate)
        {
            var result = await _domain.GetRecipientDetailsByPlateAsync(plate);

            return Ok(result ?? new
            {
                IsFrom = "Manual",
                FirstName = "",
                FatherName = "",
                LastName = "",
                PhoneNumber = "",
                PersonalId = ""
            });
        }
    }
}
