using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using trade.API.Validation;
using trade.Logic.Requests;
using trade.Logic.Services;
using trade.Shared.Dtos;

namespace trade.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _storeService;
        private readonly IGenericValidator _genericValidator;

        public StoreController(IStoreService storeService, IGenericValidator genericValidator)
        {
            _storeService = storeService;
            _genericValidator = genericValidator;
        }

        [HttpPost("paging")]
        public async Task<IActionResult> GetAllStores(PagingRequest request)
        {
            var stores = await _storeService.GetStoreAsync(request);
            return Ok(stores);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStore([FromBody] CreateStoreRequest request)
        {
            ValidationResult validationResult = await _genericValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var result = await _storeService.CreateStoreAsync(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStore(Guid id)
        {
            var result = await _storeService.DeleteStoreAsync(id);

            return Ok(result);
        }
    }
}
