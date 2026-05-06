using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBL.DTOs.ChaletOwnerDTO
{
    public class ChaletPartnerDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public double SharePercentage { get; set; }
    }

    public class ChaletWithPartnersDto
    {
        public int ChaletId { get; set; }
        public string ChaletName { get; set; }
        public List<ChaletPartnerDto> Partners { get; set; }
    }
}
