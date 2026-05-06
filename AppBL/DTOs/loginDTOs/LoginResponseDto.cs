using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBL.DTOs.loginDTOs
{
    public class LoginResponseDto
    {
        public string Token { get; set; }

        public int UserId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
    }
}
