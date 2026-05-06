using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppDAL.Entities;

namespace AppBL.DTOs.BookingDTOs
{
    public class BookingDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public int? ChaletId { get; set; }
        public string ChaletName { get; set; }
        public DateTime Date { get; set; }
        public ChaletType ChaletType { get; set; }
        public string Status { get; set; }
        public decimal ChaletPrice { get; set; }
        public decimal? ExtrasTotal { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal? Deposit { get; set; }
        public BookingPeriod Period { get; set; }
        public List<BookingExtraDto> Extras { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public ICollection<BookingNoteDTO> Notes { get; set; }
        public string? AdditionalPhone { get; set; }
        public decimal DiscountAmount { get; set; } = 0;
        public int NumOfGuests { get; set; }
        public List<PaymentDto> Payments { get; set; }
        public decimal Price { get; set; } = 0;



    }
}
