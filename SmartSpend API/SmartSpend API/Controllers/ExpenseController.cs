using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartSpend_API.Services;
using SmartSpend_API.Models;

namespace SmartSpend_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly ExpenseRepository _expenseRepository;

        public ExpenseController(ExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        // Creates a new expense
        [HttpPost("create")]
        public async Task<IActionResult> CreateExpense([FromBody] Expense expense)
        {
            bool success = await _expenseRepository.CreateExpense(expense);
            if (!success)
            {
                return BadRequest("Expense creation failed.");
            }
            return Ok("Expense created successfully.");
        }

        // Gets expenses for a user
        [HttpGet("{userID}")]
        public async Task<IActionResult> GetExpenses(int userID)
        {
            List<Expense> expenses = await _expenseRepository.GetExpensesByUserID(userID);
            if (expenses == null || expenses.Count == 0)
            {
                return NotFound("No expenses found for the user.");
            }
            return Ok(expenses);
        }

        // Gets the expenses by category ID and user ID
        [HttpGet("category/{userID}/{categoryID}")]
        public async Task<IActionResult> GetExpensesByCategory(int userID, int categoryID)
        {
            List<Expense> expenses = await _expenseRepository.GetExpensesByCategoryIDAndUserID(categoryID, userID);
            if (expenses == null || expenses.Count == 0)
            {
                return NotFound("No expenses found for this category and user.");
            }
            return Ok(expenses);
        }

        // Updates an expense
        [HttpPut("update")]
        public async Task<IActionResult> UpdateExpense([FromBody] Expense expense)
        {
            bool success = await _expenseRepository.UpdateExpense(expense);
            if (!success)
            {
                return BadRequest("Expense update failed.");
            }
            return Ok("Expense updated successfully.");
        }

        // Deletes an expense
        [HttpDelete("delete/{expenseID}")]
        public async Task<IActionResult> DeleteExpense(int expenseID)
        {
            bool success = await _expenseRepository.DeleteExpense(expenseID);
            if (!success)
            {
                return BadRequest("Expense deletion failed.");
            }
            return Ok("Expense deleted successfully.");
        }
        // Gets total expenses per category for a user
        [HttpGet("totals/user/{userID}")]
        public async Task<IActionResult> GetTotalExpensesPerCategoryForUser(int userID)
        {
            List<CategoryExpenseTotal> categoryTotals = await _expenseRepository.GetTotalExpensesPerCategoryForUser(userID);
            if (categoryTotals == null || categoryTotals.Count == 0)
            {
                return NotFound("No categories or expenses found for this user.");
            }
            return Ok(categoryTotals);
        }
    }
}
