using Domain.Contracts;
using DTO.InspectionDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AutoSystem.Controllers
{
    [Authorize(Roles = "Specialist")]
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
        public async Task<IActionResult> GetRequests()
        {
            var result = await _domain.GetMyRequestsAsync();
            return Ok(result);
        }

        [HttpPost("review")]
        public async Task<IActionResult> Review([FromBody] InspectionReviewDTO dto)
        {
            var success = await _domain.ReviewInspectionAsync(dto);
            return success ? Ok("Inspection reviewed.") : BadRequest("Review failed.");
        }

        [HttpPost("upload-docs")]
        public async Task<IActionResult> UploadDocs([FromBody] List<InspectionDocumentUploadDTO> documents)
        {
            var success = await _domain.UploadDocumentsAsync(documents);
            return success ? Ok("Documents uploaded.") : BadRequest("Upload failed.");
        }

        [HttpGet("{requestId}/documents")]
        public async Task<IActionResult> GetDocs(Guid requestId)
        {
            var docs = await _domain.GetDocumentsAsync(requestId);
            return Ok(docs);
        }

        [HttpDelete("documents/{docId}")]
        public async Task<IActionResult> DeleteDoc(Guid docId)
        {
            var result = await _domain.DeleteDocumentAsync(docId);
            return result ? Ok("Deleted.") : BadRequest("Delete failed.");
        }
    }
}
