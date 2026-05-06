using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppDAL.Entities;

namespace AppBL.DTOs
{
    public class CreatePricingDto
    {
        public ChaletType ChaletType { get; set; }
        public BookingPeriod Period { get; set; }
        public decimal Price { get; set; }
        public DayType DayType { get; set; }
    }
}
