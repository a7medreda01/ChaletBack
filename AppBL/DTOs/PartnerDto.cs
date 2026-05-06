using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBL.DTOs
{
    public class PartnerDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal ProfitPercentage { get; set; }

        public int TotalChalets { get; set; }

        public decimal TotalRevenue { get; set; }
    }
}
