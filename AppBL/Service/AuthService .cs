using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBL.Service
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using AppBL.DTOs.loginDTOs;
    using AppBL.IService;
    using AppBL.Mapper;
    using AppDAL.Entities;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;

    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;

        public AuthService(UserManager<AppUser> userManager,
                           SignInManager<AppUser> signInManager,
                           IEmailService emailService , IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _config = config;
        }

        // ✅ Login
        public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
                throw new Exception("Invalid email or password");

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);

            if (!result.Succeeded)
                throw new Exception("Invalid email or password");
            if (!user.IsActive)
                throw new Exception("Inactive account");

            // 🔹 Get Role
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();

            // 🔹 Generate Token
            var token = GenerateJwtToken(user, role);

            var refreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            return new LoginResponseDto
            {
                Token = token,
                RefreshToken = refreshToken.Token,           // ← أضف
                RefreshTokenExpiration = refreshToken.ExpiresOn, // ← أضف
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Role = role
            };

            return new LoginResponseDto
            {
                Token = token,
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Role = role
            };
        }
        // ✅ Create User (Manager only)
        public async Task<string> CreateUserAsync(CreateUserDto dto)
        {
            var user = new AppUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName,
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                throw new Exception(string.Join(",", result.Errors.Select(e => e.Description)));

            // assign role
            await _userManager.AddToRoleAsync(user, dto.Role);

            return "User created successfully";
        }

        // ✅ Forget Password
        public async Task<string> ForgetPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return "If email exists, reset link will be sent";

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var encodedToken = System.Net.WebUtility.UrlEncode(token);
            var frontendUrl = _config["FrontendUrl"];

            var resetLink = $"{frontendUrl}/reset-password?email={email}&token={encodedToken}";

            await _emailService.SendEmailAsync(
                email,
                "إعادة تعيين كلمة المرور",
                $@"
        <h2>إعادة تعيين كلمة المرور</h2>
        <p>تم طلب إعادة تعيين كلمة المرور لحسابك.</p>
        <p>لإعادة تعيين كلمة المرور اضغط على الرابط التالي:</p>
        <a href='{resetLink}'>إعادة تعيين كلمة المرور</a>
        <br/><br/>
        <p>إذا لم تقم أنت بهذا الطلب، يمكنك تجاهل هذا البريد.</p>
        <p>هذا الرابط صالح لفترة محدودة لأسباب أمنية.</p>
    "
            );

            return "تم إرسال رابط إعادة تعيين كلمة المرور إلى البريد الإلكتروني";
        }

        // ✅ Reset Password
        public async Task<string> ResetPasswordAsync(ResetPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
                throw new Exception("Invalid request");

            var result = await _userManager.ResetPasswordAsync(
                user,
                dto.Token,   // 👈 خليه زي ما هو
                dto.NewPassword
            );

            if (!result.Succeeded)
            {
                var errors = string.Join(",", result.Errors.Select(e => e.Description));
                throw new Exception(errors);
            }

            return "Password reset successfully";
        }
        private string GenerateJwtToken(AppUser user, string role)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim("uid", user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Name, user.FullName ?? ""),
        new Claim(ClaimTypes.Role, role ?? "")
    };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<List<UserWithRoleDto>> GetAllUsersAsync()
        {
            var users = _userManager.Users.ToList();

            var result = new List<UserWithRoleDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                result.Add(new UserWithRoleDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    Role = roles.FirstOrDefault(),
                    IsActive=user.IsActive
                });
            }

            return result;
        }
        public async Task<string> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new Exception("User not found");

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
                throw new Exception(string.Join(",", result.Errors.Select(e => e.Description)));

            return "User deleted successfully";
        }
















    // ─── Generate Refresh Token ───────────────────
    private RefreshToken GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomBytes),
            ExpiresOn = DateTime.UtcNow.AddDays(7),
            CreatedOn = DateTime.UtcNow
        };
    }

        // ─── Refresh Token ────────────────────────────
        public async Task<LoginResponseDto> RefreshTokenAsync(string refreshToken)
        {
            var user = await _userManager.Users
                .Include(u => u.RefreshTokens)
                .SingleOrDefaultAsync(u =>
                    u.RefreshTokens.Any(t => t.Token == refreshToken));

            if (user == null)
                throw new UnauthorizedAccessException("Invalid token");

            // ✅ الموظف inactive
            if (!user.IsActive)
            {
                // revoke كل refresh tokens
                foreach (var token in user.RefreshTokens
                             .Where(t => t.IsActive))
                {
                    token.RevokedOn = DateTime.UtcNow;
                }

                await _userManager.UpdateAsync(user);

                throw new UnauthorizedAccessException("Account is inactive");
            }

            var oldToken = user.RefreshTokens
                .SingleOrDefault(t => t.Token == refreshToken);

            if (oldToken == null)
                throw new UnauthorizedAccessException("Invalid token");

            if (!oldToken.IsActive)
                throw new UnauthorizedAccessException("Token expired");

            // revoke old token
            oldToken.RevokedOn = DateTime.UtcNow;

            // create new refresh token
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);

            await _userManager.UpdateAsync(user);

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();

            var jwt = GenerateJwtToken(user, role);

            return new LoginResponseDto
            {
                Token = jwt,
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpiration = newRefreshToken.ExpiresOn,
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Role = role
            };
        }

        // ─── Revoke (Logout) ──────────────────────────
        public async Task<bool> RevokeTokenAsync(string refreshToken)
    {
        var user = _userManager.Users
            .Include(u => u.RefreshTokens)
            .SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == refreshToken));

        if (user == null) return false;

        var token = user.RefreshTokens.Single(t => t.Token == refreshToken);
        if (!token.IsActive) return false;

        token.RevokedOn = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);
        return true;
    }
}
}
