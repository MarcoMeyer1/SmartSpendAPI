using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartSpend_API.Services;
using SmartSpend_API.Models;

namespace SmartSpend_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryRepository _categoryRepository;

        public CategoryController(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // Create a new category
        [HttpPost("create")]
        public async Task<IActionResult> CreateCategory([FromBody] Category category)
        {
            bool success = await _categoryRepository.CreateCategory(category);
            if (!success)
            {
                return BadRequest("Category creation failed.");
            }
            return Ok("Category created successfully.");
        }

        // Get categories by userID
        [HttpGet("user/{userID}")]
        public async Task<IActionResult> GetCategoriesByUserID(int userID)
        {
            List<Category> categories = await _categoryRepository.GetCategoriesByUserID(userID);
            if (categories == null || categories.Count == 0)
            {
                return NotFound("No categories found for this user.");
            }
            return Ok(categories);
        }

        // Get all categories
        [HttpGet("all")]
        public async Task<IActionResult> GetCategories()
        {
            List<Category> categories = await _categoryRepository.GetAllCategories();
            if (categories == null || categories.Count == 0)
            {
                return NotFound("No categories found.");
            }
            return Ok(categories);
        }

        // Update a category
        [HttpPut("update")]
        public async Task<IActionResult> UpdateCategory([FromBody] Category category)
        {
            bool success = await _categoryRepository.UpdateCategory(category);
            if (!success)
            {
                return BadRequest("Category update failed.");
            }
            return Ok("Category updated successfully.");
        }

        // Delete a category
        [HttpDelete("delete/{categoryID}")]
        public async Task<IActionResult> DeleteCategory(int categoryID)
        {
            bool success = await _categoryRepository.DeleteCategory(categoryID);
            if (!success)
            {
                return BadRequest("Category deletion failed.");
            }
            return Ok("Category deleted successfully.");
        }
    }
}
