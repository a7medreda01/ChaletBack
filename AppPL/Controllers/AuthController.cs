using AppBL.DTOs;
using AppBL.DTOs.loginDTOs;
using AppBL.IService;
using AppBL.Mapper;
using AppBL.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }

    // ✅ Login
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var result = await _service.LoginAsync(dto);
        return Ok(result);
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
}