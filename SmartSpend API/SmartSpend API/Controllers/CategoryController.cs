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

        // Creates a new category
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

        // Gets the categories by userID
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

        // Gets all categories
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

        // Updates a category
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

        // Deletes a category
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

        // Gets the category by ID
        [HttpGet("{categoryID}")]
        public async Task<IActionResult> GetCategoryByID(int categoryID)
        {
            Category category = await _categoryRepository.GetCategoryByID(categoryID);
            if (category == null)
            {
                return NotFound("Category not found.");
            }
            return Ok(category);
        }

    }
}
