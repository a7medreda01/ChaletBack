using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppDAL.Entities;

namespace AppBL.DTOs.BookingDTOs
{

        public class CreateBookingDto
        {
            public string CustomerName { get; set; }
            public string Phone { get; set; }
            public DateTime Date { get; set; }
            public BookingPeriod Period { get; set; }

        public ChaletType ChaletType { get; set; } // ✅ الجديد
        //public int? ChaletId { get; set; }
        public int NumOfGuests { get; set; }
            public List<AddExtraTOBook>? Extras { get; set; }
        public string? Note { get; set; }
        public string? AdditionalPhone { get; set; }
        public decimal DiscountAmount { get; set; } = 0;
        public decimal Price { get; set; } = 0;
        public string? UserName { get; set; }

    }

}
