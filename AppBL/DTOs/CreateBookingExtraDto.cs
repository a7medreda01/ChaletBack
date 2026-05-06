using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBL.DTOs
{
    public class CreateBookingExtraDto
    {
        public int BookingId { get; set; }
        public int ExtraId { get; set; }
        public int Quantity { get; set; }
    }
}
