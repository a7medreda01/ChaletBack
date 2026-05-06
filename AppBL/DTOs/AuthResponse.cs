using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBL.DTOs
{
    public class AuthResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public DateTime AccessTokenExpiration { get; set; }
        public bool MustChangePassword { get; set; }

        public string Email { get; set; }
        public string UserId { get; set; }
        public List<string> Roles { get; set; }
    }
}
