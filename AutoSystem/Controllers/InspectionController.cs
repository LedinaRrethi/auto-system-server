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


        //[Authorize(Roles = "Specialist")]
        //[HttpPost("review")]
        //public async Task<IActionResult> Review([FromBody] InspectionReviewDTO dto)
        //{
        //    var success = await _domain.ReviewInspectionAsync(dto);
        //    return success ? Ok("Inspection reviewed.") : BadRequest("Review failed.");
        //}

        //[Authorize(Roles = "Specialist")]
        //[HttpPost("upload-docs")]
        //public async Task<IActionResult> UploadDocs([FromBody] List<InspectionDocumentUploadDTO> documents)
        //{
        //    var success = await _domain.UploadDocumentsAsync(documents);
        //    return success ? Ok("Documents uploaded.") : BadRequest("Upload failed.");
        //}

        //[Authorize(Roles = "Specialist")]
        //[HttpGet("{requestId}/documents")]
        //public async Task<IActionResult> GetDocs(Guid requestId)
        //{
        //    var docs = await _domain.GetDocumentsAsync(requestId);
        //    return Ok(docs);
        //}

        //[Authorize(Roles = "Specialist")]
        //[HttpDelete("documents/{docId}")]
        //public async Task<IActionResult> DeleteDoc(Guid docId)
        //{
        //    var result = await _domain.DeleteDocumentAsync(docId);
        //    return result ? Ok("Deleted.") : BadRequest("Delete failed.");
        //}


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
