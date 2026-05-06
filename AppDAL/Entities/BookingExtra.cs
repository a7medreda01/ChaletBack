using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDAL.Entities
{
    public class BookingExtra
    {
        public int Id { get; set; }

        public int BookingId { get; set; }
        public Booking Booking { get; set; }

        public int ExtraId { get; set; }
        public Extra Extra { get; set; }

        public decimal Price { get; set; } // snapshot
        public int Quantity { get; set; }
    }
}
