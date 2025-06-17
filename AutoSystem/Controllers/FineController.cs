using Domain.Contracts;
using DTO.FineDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            var success = await _domain.CreateFineAsync(dto, userId, ip ?? "unknown");

            return success ? Ok("Gjoba u vendos.") : BadRequest("Gabim në regjistrim.");
        }

        [Authorize(Roles = "Individ")]

        [HttpGet("my-fines")]
        public async Task<IActionResult> GetMyFines([FromQuery] FineFilterDTO filter, int page = 1, int pageSize = 10)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _domain.GetMyFinesAsync(userId, filter, page, pageSize);
            return Ok(result);
        }

        [Authorize(Roles = "Police")]
        [HttpGet("search")]
        public async Task<IActionResult> SearchFines([FromQuery] string plate, int page = 1, int pageSize = 10)
        {
            var result = await _domain.SearchFinesByPlateAsync(plate, page, pageSize);
            return Ok(result);
        }


        [Authorize(Roles = "Police")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllFines([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _domain.GetAllFinesAsync(page, pageSize);
            return Ok(result);
        }


        [Authorize(Roles = "Police")]
        [HttpGet("owner-details")]
        public async Task<IActionResult> GetVehicleOwnerDetails([FromQuery] string plate)
        {
            var result = await _domain.GetVehicleOwnerInfoAsync(plate);
            if (result == null)
                return NotFound("Vehicle not ofund.");

            return Ok(result);
        }

    }

}
