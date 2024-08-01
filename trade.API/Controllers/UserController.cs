using Microsoft.AspNetCore.Mvc;
using trade.Logic.Requests;
using trade.Logic.Services;
using FluentValidation.Results;
using trade.API.Validation;

namespace trade.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserServices _userService;
    private readonly IGenericValidator _genericValidator;

    public UserController(IUserServices userServices, IGenericValidator genericValidator)
    {
        _userService = userServices;
        _genericValidator = genericValidator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        try
        {
            ValidationResult validationResult = await _genericValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var result = await _userService.RegisterAsync(request);

            if (result == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] UserLoginRequest request)
    {
        try
        {
            ValidationResult validationResult = await _genericValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var token = await _userService.LoginAsync(request);

            if (token == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            return Ok(token);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}