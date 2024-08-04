using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;
using trade.API.Validation;
using trade.Logic.Interfaces;
using trade.Logic.Request;
using trade.Shared.Dtos;

namespace trade.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IGenericValidator _genericValidator;

        public CategoryController(ICategoryService categoryService, IGenericValidator genericValidator)
        {
            _categoryService = categoryService;
            _genericValidator = genericValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetListCategoryAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound(new { message = "Category not found" });

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CategoryRequest request)
        {
            ValidationResult validationResult = await _genericValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var result = await _categoryService.AddCategoryAsync(request);
            return CreatedAtAction(nameof(GetCategoryById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditCategory(Guid id, [FromBody] CategoryRequest request)
        {
            ValidationResult validationResult = await _genericValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var result = await _categoryService.EditCategoryAsync(id, request);
            if (result == null)
                return NotFound(new { message = "Category not found" });

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            if (result == null)
                return NotFound(new { message = "Category not found" });

            return Ok(result);
        }

        [HttpPost("paging")]
        public async Task<IActionResult> Paging([FromBody] PagingRequest request)
        {
            try
            {
                var result = await _categoryService.GetPagingCategoryAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
