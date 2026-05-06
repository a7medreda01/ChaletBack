using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppBL.DTOs.loginDTOs;

namespace AppBL.IService
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginDto dto);
        Task<string> CreateUserAsync(CreateUserDto dto);
        Task<string> ForgetPasswordAsync(string email);
        Task<string> ResetPasswordAsync(ResetPasswordDto dto);
        Task<List<UserWithRoleDto>> GetAllUsersAsync();
        Task<string> DeleteUserAsync(string userId);
    }
}
