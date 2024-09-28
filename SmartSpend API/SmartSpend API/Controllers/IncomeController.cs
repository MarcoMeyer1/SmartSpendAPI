using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartSpend_API.Services;
using SmartSpend_API.Models;

namespace SmartSpend_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncomeController : ControllerBase
    {
        private readonly IncomeRepository _incomeRepository;

        public IncomeController(IncomeRepository incomeRepository)
        {
            _incomeRepository = incomeRepository;
        }

        // Create a new income entry
        [HttpPost("create")]
        public async Task<IActionResult> CreateIncome([FromBody] Income income)
        {
            if (income == null || income.Amount <= 0)
            {
                return BadRequest("Invalid income data.");
            }

            bool success = await _incomeRepository.CreateIncome(income);
            if (!success)
            {
                return BadRequest("Income creation failed.");
            }
            return Ok("Income created successfully.");
        }

        // Get income entries for a user
        [HttpGet("{userID}")]
        public async Task<IActionResult> GetIncome(int userID)
        {
            List<Income> incomeList = await _incomeRepository.GetIncomeByUserID(userID);
            if (incomeList == null || incomeList.Count == 0)
            {
                return NotFound("No income entries found for the user.");
            }
            return Ok(incomeList);
        }

        // Update an income entry
        [HttpPut("update")]
        public async Task<IActionResult> UpdateIncome([FromBody] Income income)
        {
            bool success = await _incomeRepository.UpdateIncome(income);
            if (!success)
            {
                return BadRequest("Income update failed.");
            }
            return Ok("Income updated successfully.");
        }

        // Delete an income entry
        [HttpDelete("delete/{incomeID}")]
        public async Task<IActionResult> DeleteIncome(int incomeID)
        {
            bool success = await _incomeRepository.DeleteIncome(incomeID);
            if (!success)
            {
                return BadRequest("Income deletion failed.");
            }
            return Ok("Income deleted successfully.");
        }
    }
}
