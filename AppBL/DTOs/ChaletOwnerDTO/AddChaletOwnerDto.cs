using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBL.DTOs.ChaletOwnerDTO
{
    public class AddChaletOwnerDto
    {
        public int ChaletId { get; set; }
        public int UserId { get; set; }
        public decimal SharePercentage { get; set; }
    }
}
