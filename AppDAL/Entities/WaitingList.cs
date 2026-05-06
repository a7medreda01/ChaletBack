using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDAL.Entities
{
    public class WaitingList
    {
        public int Id { get; set; }

        public string CustomerName { get; set; }
        public string Phone { get; set; }

        public int? ChaletId { get; set; }
        public Chalet? Chalet { get; set; }
        public ChaletType ChaletType { get; set; }
        public DateTime Date { get; set; }
        public BookingPeriod Period { get; set; }

        public WaitingStatus Status { get; set; }= WaitingStatus.Pending;
        public decimal ChaletPrice { get; set; } = 0;
        public decimal? ExtrasTotal { get; set; } = 0;
        public decimal TotalPrice { get; set; }
        public decimal? Deposit { get; set; }
        public int NumOfGuests { get; set; } = 1;
        public ICollection<BookingExtra> BookingExtras { get; set; }

        public ICollection<Payment> Payments { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ExpireAt { get; set; }
        public int CreatedByUserId { get; set; }
        public AppUser CreatedByUser { get; set; }
        //public string? Notes { get; set; }
        public ICollection<BookingNote>? Notes { get; set; }
        public string? AdditionalPhone { get; set; }
        public decimal DiscountAmount { get; set; } = 0;
        public decimal Price { get; set; } = 0;

    }
}
