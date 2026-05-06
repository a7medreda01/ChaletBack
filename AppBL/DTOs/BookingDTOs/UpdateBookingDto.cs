using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBL.DTOs.BookingDTOs
{
    public class UpdateBookingDto
    {
        public int BookingId { get; set; }

        public string CustomerName { get; set; }
        public string Phone { get; set; }

        //public decimal TotalPrice { get; set; }
        public int PayMoney { get; set; } 
        public decimal? Deposit { get; set; }
        public string? AdditionalPhone { get; set; }
        public decimal DiscountAmount { get; set; } = 0;

        // IDs بتاعة الـ BookingExtras اللي عايز تمسحها
        public List<int> RemovedExtraIds { get; set; } = new();
    }
}
