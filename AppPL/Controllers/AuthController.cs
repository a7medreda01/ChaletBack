using AppBL.DTOs;
using AppBL.DTOs.loginDTOs;
using AppBL.IService;
using AppBL.Mapper;
using AppBL.Service;
using AppDAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;
    private readonly UserManager<AppUser> _userManager;

    public AuthController(IAuthService service,UserManager<AppUser> userManager)
    {
        _service = service;
        this._userManager = userManager;
    }

    // ✅ Login
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        {
            try
            {
                var result = await _service.LoginAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
    // ✅ Create User (Manager only)
    //[Authorize(Roles = Roles.Manager)]
    [HttpPost("create-user")]
    public async Task<IActionResult> CreateUser(CreateUserDto dto)
    {
        var result = await _service.CreateUserAsync(dto);
        return Ok(result);
    }

    [HttpPost("forget-password")]
    public async Task<IActionResult> ForgetPassword(ForgotPasswordDto dto)
    {
        var result = await _service.ForgetPasswordAsync(dto.Email);
        return Ok(result);
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
    {
        var result = await _service.ResetPasswordAsync(dto);
        return Ok(new { message = "Password reset successfully" });
    }
    [HttpDelete("user/{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var result = await _service.DeleteUserAsync(id);
        return Ok(result);
    }
    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var result = await _service.GetAllUsersAsync();
        return Ok(result);
    }

    [HttpPut("toggle-active/{userId}")]
    public async Task<IActionResult> ToggleActive(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
            return NotFound(new
            {
                message = "User not found"
            });

        // منع تعطيل نفسك (اختياري)
        var currentUserId = User.FindFirst("uid")?.Value;
        if (currentUserId == userId.ToString())
        {
            return BadRequest(new
            {
                message = "You cannot deactivate your own account"
            });
        }

        user.IsActive = !user.IsActive;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            return BadRequest(new
            {
                message = "Failed to update user status",
                errors = result.Errors.Select(x => x.Description)
            });
        }

        return Ok(new
        {
            message = user.IsActive
                ? "User activated successfully"
                : "User deactivated successfully",

            isActive = user.IsActive
        });
    }
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(
        [FromBody] RefreshTokenDto dto)
    {
        try
        {
            var result =
                await _service.RefreshTokenAsync(
                    dto.RefreshToken);

            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new
            {
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }
    [HttpPost("revoke-token")]
    public async Task<IActionResult> RevokeToken([FromBody] RefreshTokenDto dto)
    {
        var result = await _service.RevokeTokenAsync(dto.RefreshToken);
        return result ? Ok(new { message = "Token revoked" })
                      : BadRequest(new { message = "Invalid token" });
    }
}