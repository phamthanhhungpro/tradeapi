using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;
using trade.API.Validation;
using trade.Logic.Interfaces;
using trade.Logic.Request;

namespace trade.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IGenericValidator _genericValidator;

        public ProductController(IProductService productService, IGenericValidator genericValidator)
        {
            _productService = productService;
            _genericValidator = genericValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetListProductAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound(new { message = "Product not found" });

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductRequest request)
        {
            ValidationResult validationResult = await _genericValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var result = await _productService.AddProductAsync(request);
            return CreatedAtAction(nameof(GetProductById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditProduct(Guid id, [FromBody] ProductRequest request)
        {
            ValidationResult validationResult = await _genericValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var result = await _productService.EditProductAsync(id, request);
            if (result == null)
                return NotFound(new { message = "Product not found" });

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (result == null)
                return NotFound(new { message = "Product not found" });

            return Ok(result);
        }
    }
}
