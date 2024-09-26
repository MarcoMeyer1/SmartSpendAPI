using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SmartSpend_API.Services;
using SmartSpend_API.Models;

namespace SmartSpend_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly SettingsRepository _settingsRepository;

        public SettingsController(SettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        // Get settings for a specific user
        [HttpGet("{userID}")]
        public async Task<IActionResult> GetSettings(int userID)
        {
            Settings settings = await _settingsRepository.GetSettingsByUserID(userID);
            if (settings == null)
            {
                return NotFound("Settings not found for the user.");
            }
            return Ok(settings);
        }

        // Update settings
        [HttpPut("update")]
        public async Task<IActionResult> UpdateSettings([FromBody] Settings settings)
        {
            bool success = await _settingsRepository.UpdateSettings(settings);
            if (!success)
            {
                return BadRequest("Settings update failed.");
            }
            return Ok("Settings updated successfully.");
        }

        // Add settings for a new user
        [HttpPost("add")]
        public async Task<IActionResult> AddSettings([FromBody] Settings settings)
        {
            bool success = await _settingsRepository.AddSettings(settings);
            if (!success)
            {
                return BadRequest("Settings creation failed.");
            }
            return Ok("Settings created successfully.");
        }

        // Delete settings for a user
        [HttpDelete("delete/{userID}")]
        public async Task<IActionResult> DeleteSettings(int userID)
        {
            bool success = await _settingsRepository.DeleteSettings(userID);
            if (!success)
            {
                return BadRequest("Settings deletion failed.");
            }
            return Ok("Settings deleted successfully.");
        }
    }
}
