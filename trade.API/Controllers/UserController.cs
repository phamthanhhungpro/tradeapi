using Microsoft.AspNetCore.Mvc;
using trade.Logic.Requests;
using trade.Logic.Services;
using FluentValidation.Results;
using trade.API.Validation;
using Microsoft.AspNetCore.Authorization;
using trade.Shared.Dtos;

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

            return Ok(result);
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
                return Unauthorized(new { message = "Invalid token" });

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

            return Ok(result);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    [HttpPost("paging")]
    public async Task<IActionResult> Paging([FromBody] PagingRequest request)
    {
        try
        {
            var result = await _userService.GetPagingUser(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var result = await _userService.DeleteUser(id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
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

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUserInfo(Guid id, [FromBody] UpdateUserInfoRequest request)
    {
        try
        {
            ValidationResult validationResult = await _genericValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var result = await _userService.UpdateUserInfo(id, request);

            if (result == null)
                return NotFound(new { message = "User not found" });

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}