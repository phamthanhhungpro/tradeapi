using Microsoft.AspNetCore.Mvc;
using trade.Logic.Requests;
using trade.Logic.Services;
using FluentValidation.Results;
using trade.API.Validation;
using Microsoft.AspNetCore.Authorization;

namespace trade.API.Controllers;
[Authorize]
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

    [AllowAnonymous]
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

    [AllowAnonymous]
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

            var result = await _userService.LoginAsync(request);

            if (result == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(result.Token);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    [AllowAnonymous]
    [HttpPost("login-with-token")]
    public async Task<IActionResult> GetUserInfoFromToken([FromBody] UserLoginWithTokenRequest request)
    {
        try
        {
            var result = await _userService.GetUserInfoFromToken(request.AccessToken);
            if (result == null)
                return BadRequest(new { message = "Invalid token" });

            return Ok(result);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordRequest request)
    {
        try
        {
            ValidationResult validationResult = await _genericValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var result = await _userService.ChangePasswordAsync(request);

            if (result == null)
                return BadRequest(new { message = "Password change failed" });

            return Ok(result.Message);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> LogoutAsync([FromBody] Guid userId)
    {
        try
        {
            var result = await _userService.LogoutAsync(userId);

            if (result == null)
                return BadRequest(new { message = "Logout failed" });

            return Ok(result.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}