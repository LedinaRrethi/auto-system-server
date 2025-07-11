using Azure.Core;
using Domain.Contracts;
using DTO;
using DTO.InspectionDTO;
using JasperFx.CodeGeneration.Frames;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AutoSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InspectionRequestController : ControllerBase
    {
        private readonly IInspectionRequestDomain _domain;

        public InspectionRequestController(IInspectionRequestDomain domain)
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


        //[Authorize(Roles = "Individ")]
        //[HttpGet("my-requests")]
        //public async Task<IActionResult> GetMyRequests()
        //{
        //    var result = await _domain.GetRequestsByCurrentUserAsync();
        //    return Ok(result);
        //}

        [Authorize(Roles = "Individ")]
        [HttpGet("my-requests-paged")]
        public async Task<IActionResult> GetPagedRequests([FromQuery] PaginationDTO dto)
        {
            var result = await _domain.GetCurrentUserPagedInspectionRequestsAsync(dto);
            result.Message = !result.Items.Any() ? "No inspection request found." : "Success";
            return Ok(result);
        }


    }
}
