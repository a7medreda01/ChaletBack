using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBL.DTOs
{
    // AppBL/DTOs/CustomerDto.cs
    public class CustomerDto
    {
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public int BookingsCount { get; set; }
        public string LastBookingDate { get; set; }
    }
}
