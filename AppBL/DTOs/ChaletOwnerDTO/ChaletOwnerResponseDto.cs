using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBL.DTOs.ChaletOwnerDTO
{
    public class ChaletOwnerResponseDto
    {
        public int Id { get; set; }
        public int ChaletId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public decimal SharePercentage { get; set; }
    }
}
