using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartSpend_API.Services;
using SmartSpend_API.Models;

namespace SmartSpend_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationRepository _notificationRepository;

        public NotificationController(NotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        // Create a new notification
        [HttpPost("create")]
        public async Task<IActionResult> CreateNotification([FromBody] Notification notification)
        {
            bool success = await _notificationRepository.CreateNotification(notification);
            if (!success)
            {
                return BadRequest("Notification creation failed.");
            }
            return Ok("Notification created successfully.");
        }

        // Get notifications for a user
        [HttpGet("{userID}")]
        public async Task<IActionResult> GetNotifications(int userID)
        {
            List<Notification> notifications = await _notificationRepository.GetNotificationsByUserID(userID);
            if (notifications == null || notifications.Count == 0)
            {
                return NotFound("No notifications found for the user.");
            }
            return Ok(notifications);
        }

        // Update a notification
        [HttpPut("update")]
        public async Task<IActionResult> UpdateNotification([FromBody] Notification notification)
        {
            bool success = await _notificationRepository.UpdateNotification(notification);
            if (!success)
            {
                return BadRequest("Notification update failed.");
            }
            return Ok("Notification updated successfully.");
        }

        // Delete a notification
        [HttpDelete("delete/{notificationID}")]
        public async Task<IActionResult> DeleteNotification(int notificationID)
        {
            bool success = await _notificationRepository.DeleteNotification(notificationID);
            if (!success)
            {
                return BadRequest("Notification deletion failed.");
            }
            return Ok("Notification deleted successfully.");
        }
    }
}
