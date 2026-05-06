using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBL.DTOs
{
    public class PricingDto
    {
        public int Id { get; set; }
        public string ChaletType { get; set; }
        public string Period { get; set; }
        public decimal Price { get; set; }
        public string DayType { get; set; }
    }
}
