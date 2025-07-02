using Domain.Contracts;
using DTO.NotificationDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AutoSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationDomain _notificationDomain;

        public NotificationController(INotificationDomain notificationDomain)
        {
            _notificationDomain = notificationDomain;
        }

        private string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }   

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetUserId();
            if(string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID is required.");
            }
            var result = await _notificationDomain.GetAllNotificationsAsync(userId);
            return Ok(result);
        }

        [HttpGet("unseen")]
        public async Task<IActionResult> GetUnseen()
        {
            var userId = GetUserId();
            if(string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID is required.");
            }
            var result = await _notificationDomain.GetUnseenNotificationsAsync(userId);
            return Ok(result);
        }

        [HttpGet("count-unseen")]
        public async Task<IActionResult> CountUnseen()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID is required.");
            }
            var count = await _notificationDomain.CountUnseenNotificationsAsync(userId);
            return Ok(count);
        }

        [HttpPut("mark-all-seen")]
        public async Task<IActionResult> MarkAllAsSeen()
        {
            var userId = GetUserId();
            if(string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID is required.");
            }
            await _notificationDomain.MarkAllNotificationsAsSeenAsync(userId);
            return NoContent();
        }

        [HttpPut("mark-one-seen/{notificationId}")]
        public async Task<IActionResult> MarkOneAsSeen(Guid notificationId)
        {
            await _notificationDomain.MarkOneNotificationAsSeenAsync(notificationId);
            return NoContent();
        }
    }
}
