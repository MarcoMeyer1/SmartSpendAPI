using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartSpend_API.Services;
using SmartSpend_API.Models;

namespace SmartSpend_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetailedViewController : ControllerBase
    {
        private readonly DetailedViewRepository _detailedViewRepository;

        public DetailedViewController(DetailedViewRepository detailedViewRepository)
        {
            _detailedViewRepository = detailedViewRepository;
        }

        // Creates a new detailed view
        [HttpPost("create")]
        public async Task<IActionResult> CreateDetailedView([FromBody] DetailedView detailedView)
        {
            bool success = await _detailedViewRepository.CreateDetailedView(detailedView);
            if (!success)
            {
                return BadRequest("Detailed view creation failed.");
            }
            return Ok("Detailed view created successfully.");
        }

        // Gets detailed views for a user
        [HttpGet("{userID}")]
        public async Task<IActionResult> GetDetailedViews(int userID)
        {
            List<DetailedView> detailedViews = await _detailedViewRepository.GetDetailedViewsByUserID(userID);
            if (detailedViews == null || detailedViews.Count == 0)
            {
                return NotFound("No detailed views found for the user.");
            }
            return Ok(detailedViews);
        }

        // Updates a detailed view
        [HttpPut("update")]
        public async Task<IActionResult> UpdateDetailedView([FromBody] DetailedView detailedView)
        {
            bool success = await _detailedViewRepository.UpdateDetailedView(detailedView);
            if (!success)
            {
                return BadRequest("Detailed view update failed.");
            }
            return Ok("Detailed view updated successfully.");
        }

        // Deletes a detailed view
        [HttpDelete("delete/{detailedViewID}")]
        public async Task<IActionResult> DeleteDetailedView(int detailedViewID)
        {
            bool success = await _detailedViewRepository.DeleteDetailedView(detailedViewID);
            if (!success)
            {
                return BadRequest("Detailed view deletion failed.");
            }
            return Ok("Detailed view deleted successfully.");
        }
    }
}
