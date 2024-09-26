using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartSpend_API.Services;
using SmartSpend_API.Models;

namespace SmartSpend_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReminderController : ControllerBase
    {
        private readonly ReminderRepository _reminderRepository;

        public ReminderController(ReminderRepository reminderRepository)
        {
            _reminderRepository = reminderRepository;
        }

        // Create a new reminder
        [HttpPost("create")]
        public async Task<IActionResult> CreateReminder([FromBody] Reminder reminder)
        {
            bool success = await _reminderRepository.CreateReminder(reminder);
            if (!success)
            {
                return BadRequest("Reminder creation failed.");
            }
            return Ok("Reminder created successfully.");
        }

        // Get reminders for a user
        [HttpGet("{userID}")]
        public async Task<IActionResult> GetReminders(int userID)
        {
            List<Reminder> reminders = await _reminderRepository.GetRemindersByUserID(userID);
            if (reminders == null || reminders.Count == 0)
            {
                return NotFound("No reminders found for the user.");
            }
            return Ok(reminders);
        }

        // Update a reminder
        [HttpPut("update")]
        public async Task<IActionResult> UpdateReminder([FromBody] Reminder reminder)
        {
            bool success = await _reminderRepository.UpdateReminder(reminder);
            if (!success)
            {
                return BadRequest("Reminder update failed.");
            }
            return Ok("Reminder updated successfully.");
        }

        // Delete a reminder
        [HttpDelete("delete/{reminderID}")]
        public async Task<IActionResult> DeleteReminder(int reminderID)
        {
            bool success = await _reminderRepository.DeleteReminder(reminderID);
            if (!success)
            {
                return BadRequest("Reminder deletion failed.");
            }
            return Ok("Reminder deleted successfully.");
        }
    }
}
