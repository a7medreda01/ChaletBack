using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBL.DTOs.BookingDTOs
{
    public class BookingExtraDto
    {
        public int Id { get; set; }

        public int BookingId { get; set; }

        public int ExtraId { get; set; }

        public string ExtraName { get; set; } // 👈 مهم للعرض

        public decimal Price { get; set; } // snapshot

        public int Quantity { get; set; }

        public decimal Total => Price * Quantity; // 👈 محسوبة
    }
}
