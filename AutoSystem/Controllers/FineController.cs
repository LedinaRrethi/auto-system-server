using Domain.Contracts;
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

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User is not authenticated.");

            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            try
            {
                var success = await _domain.CreateFineAsync(dto, userId, ip);
                if (!success)
                    return BadRequest("An error occurred while registering the fine.");

                return Ok(new { message = "Fine successfully issued." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    error = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "An error occurred while creating the fine.",
                    details = ex.Message
                });
            }
        }

        [Authorize(Roles = "Individ")]
        [HttpGet("my-fines")]
        public async Task<IActionResult> GetMyFines([FromQuery] FineFilterDTO filter)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User is not authenticated.");

            var result = await _domain.GetMyFinesAsync(userId, filter);

            result.Message = !result.Items.Any() ? "No fines found." : "Success";
            return Ok(result);
        }

        [Authorize(Roles = "Police")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllFines([FromQuery] FineFilterDTO filter)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User is not authenticated.");

            var result = await _domain.GetAllFinesAsync(userId , filter);
            result.Message = !result.Items.Any() ? "There are no fines." : "Success";
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
