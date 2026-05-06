using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AppDAL.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string? FullName { get; set; }

        public bool MustChangePassword { get; set; } = true;

        // بيانات الشريك
        public decimal Balance { get; set; } = 0;
        public decimal TotalRevenue { get; set; } = 0;

        public ICollection<ChaletOwner>? ChaletOwners { get; set; }

        // 🔐 Refresh Tokens
        public List<RefreshToken>? RefreshTokens { get; set; } = new();
    }
}

