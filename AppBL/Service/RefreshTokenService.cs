using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppDAL.Entities;

namespace AppBL.Service
{
    public class RefreshTokenService
    {
        public RefreshToken GenerateRefreshToken(string userId)
        {
            return new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                ExpiresOn = DateTime.UtcNow.AddDays(7),
                UserId = userId,
                IsRevoked = false
            };
        }
    }
}
