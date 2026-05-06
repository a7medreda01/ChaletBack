using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDAL.Entities
{
    public class Pricing
    {
        public int Id { get; set; }
        public ChaletType ChaletType { get; set; }
        public BookingPeriod Period { get; set; }
        public decimal Price { get; set; }
        public DayType DayType { get; set; }

    }
}
