using Domain.Concrete;
using Domain.Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AutoSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IAdminDomain _adminDomain;
        private readonly IAdminVehicleRequestDomain _adminRequestDomain;
        private readonly IInspectionRequestDomain _inspectionRequestDomain;
        private readonly IFineDomain _fineDomain;
        private readonly IVehicleRequestDomain _vehicleRequestDomain;
        private readonly INotificationDomain _notificationDomain;
        private readonly UserManager<Auto_Users> _userManager;

        private string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public DashboardController(UserManager<Auto_Users> userManager, IVehicleRequestDomain vehicleRequestDomain, IAdminDomain adminDomain , IAdminVehicleRequestDomain adminRequestDomain, IInspectionRequestDomain inspectionRequestDomain , IFineDomain fineDomain, INotificationDomain notificationDomain)
        {
            _userManager = userManager;
            _adminDomain = adminDomain;
            _adminRequestDomain = adminRequestDomain;
            _inspectionRequestDomain = inspectionRequestDomain;
            _fineDomain = fineDomain;
            _vehicleRequestDomain = vehicleRequestDomain;
            _notificationDomain = notificationDomain;

        }

        [HttpGet("admin")]
        public async Task<IActionResult> GetAdminDashboard()
        {
            try {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User ID is required.");
                }

                var userCounts = await _adminDomain.GetUserCountAsync();
                var vehicleCounts = await _adminRequestDomain.GetVehicleRequestCountAsync();

                var notificationCount = await _notificationDomain.CountUnseenNotificationsAsync(userId);

                return Ok(new
                {
                    totalUsers = userCounts,
                    totalVehicleRequests = vehicleCounts,
                    notifications = notificationCount

                });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "An error occurred while loading admin dashboard.",
                    details = ex.Message
                });
            }
        }

        [HttpGet("specialist")]
        public async Task<IActionResult> GetSpecialistDashboard()
        {
            try {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User ID is required.");
                }

                var specialist = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (specialist == null || specialist.IDFK_Directory == null)
                {
                    return BadRequest("Specialist or Directorate not found.");
                }

                var directoryId = specialist.IDFK_Directory.Value;

                var inspectionCounts = await _inspectionRequestDomain.GetInspectionStatusCountAsync(directoryId);
                var notificationCount = await _notificationDomain.CountUnseenNotificationsAsync(userId);

                return Ok(new
                {
                    inspections = inspectionCounts,
                    notifications = notificationCount
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "An error occurred while loading specialist dashboard.",
                    details = ex.Message
                });
            }

        }

        [HttpGet("police")]
        public async Task<IActionResult> GetPoliceDashboard()
        {
            try {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User ID is required.");
                }

                var fineCounts = await _fineDomain.GetFinesCountAsync(userId);
                var notificationCount = await _notificationDomain.CountUnseenNotificationsAsync(userId);
                return Ok(new
                {
                    fines = fineCounts,
                    notifications = notificationCount
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "An error occurred while loading police dashboard.",
                    details = ex.Message
                });
            }

        }

        [HttpGet("user")]
        public async Task<IActionResult> GetUserDashboard()
        {
            try {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                var finesCount = await _fineDomain.GetFinesCountForUserAsync(userId);
                var vehicleRequestsCount = await _vehicleRequestDomain.GetVehicleRequestCountAsync(userId);
                var inspectionsCount = await _inspectionRequestDomain.GetInspectionRequestStatusForUserAsync(userId);
                var notificationCount = await _notificationDomain.CountUnseenNotificationsAsync(userId);

                return Ok(new
                {
                    myFinesCount = finesCount,
                    myVehicleRequestsCount = vehicleRequestsCount,
                    myInspectionRequestCount = inspectionsCount,
                    notifications = notificationCount

                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "An error occurred while loading user dashboard.",
                    details = ex.Message
                });
            }
        }
    }
}
