using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppBL.DTOs.BookingDTOs;
using AppDAL.Entities;

namespace AppBL.DTOs
{
    public class WaitingListDto
    {
      public int Id { get; set; }
    public string CustomerName { get; set; }
    public string Phone { get; set; }
    public int ChaletId { get; set; }
    public ChaletType ChaletType { get; set; }
        public string ChaletName { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; }
    public decimal ChaletPrice { get; set; }
    public decimal? ExtrasTotal { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal? Deposit { get; set; }
    public BookingPeriod Period { get; set; }
    public List<BookingExtraDto> Extras { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? CreatedAt { get; set; }
    //public string? Notes { get; set; }
    public int NumOfGuests { get; set; }
        public ICollection<BookingNoteDTO>? Notes { get; set; }
        public string? AdditionalPhone { get; set; }
        public decimal DiscountAmount { get; set; } = 0;
        public decimal Price { get; set; } = 0;


    }
}
