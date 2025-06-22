using Domain.Contracts;
using DTO.InspectionDTO;
using JasperFx.CodeGeneration.Frames;
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

        [Authorize(Roles = "Individ")]
        [HttpPost("request")]
        public async Task<IActionResult> CreateRequest([FromBody] InspectionRequestCreateDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();

            try
            {
                var success = await _domain.CreateInspectionRequestAsync(dto);
                return success ? Ok("Request submitted.") : BadRequest("Submission failed.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
