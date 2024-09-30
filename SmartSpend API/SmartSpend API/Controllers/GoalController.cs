using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartSpend_API.Services;
using SmartSpend_API.Models;

namespace SmartSpend_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoalController : ControllerBase
    {
        private readonly GoalRepository _goalRepository;

        public GoalController(GoalRepository goalRepository)
        {
            _goalRepository = goalRepository;
        }

        // Creates a new goal
        [HttpPost("create")]
        public async Task<IActionResult> CreateGoal([FromBody] Goal goal)
        {
            bool success = await _goalRepository.CreateGoal(goal);
            if (!success)
            {
                return BadRequest("Goal creation failed.");
            }
            return Ok("Goal created successfully.");
        }

        // Gets goals for a user
        [HttpGet("{userID}")]
        public async Task<IActionResult> GetGoals(int userID)
        {
            List<Goal> goals = await _goalRepository.GetGoalsByUserID(userID);
            if (goals == null || goals.Count == 0)
            {
                return NotFound("No goals found for the user.");
            }
            return Ok(goals);
        }

        // Updates a goal
        [HttpPut("update")]
        public async Task<IActionResult> UpdateGoal([FromBody] Goal goal)
        {
            bool success = await _goalRepository.UpdateGoal(goal);
            if (!success)
            {
                return BadRequest("Goal update failed.");
            }
            return Ok("Goal updated successfully.");
        }

        // Deletes a goal
        [HttpDelete("delete/{goalID}")]
        public async Task<IActionResult> DeleteGoal(int goalID)
        {
            bool success = await _goalRepository.DeleteGoal(goalID);
            if (!success)
            {
                return BadRequest("Goal deletion failed.");
            }
            return Ok("Goal deleted successfully.");
        }
    }
}
