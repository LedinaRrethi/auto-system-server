using Domain.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectorateController : ControllerBase
    {

        private readonly IDirectorateDomain _directorateDomain;

        public DirectorateController(IDirectorateDomain directorateDomain)
        {
            _directorateDomain = directorateDomain;
        }

        [HttpGet]
        [AllowAnonymous] // TODO: vendos authorize me vone
        public async Task<IActionResult> GetAllActive()
        {
            try
            {
                var list = await _directorateDomain.GetAllActiveAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while fetching data.", details = ex.Message });
            }
        }

    }
}
