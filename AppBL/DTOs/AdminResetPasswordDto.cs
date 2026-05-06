using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBL.DTOs
{
    public class AdminResetPasswordDto
    {
        public int UserId { get; set; }
        public string NewPassword { get; set; }
    }
}
