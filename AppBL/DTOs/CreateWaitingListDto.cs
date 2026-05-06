using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppDAL.Entities;

namespace AppBL.DTOs
{
    public class CreateWaitingListDto
    {
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        //public int? ChaletId { get; set; }
        public ChaletType ChaletType { get; set; }
        public DateTime Date { get; set; }
        public BookingPeriod Period { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int CreatedByUserId { get; set; }
        public AppUser CreatedByUser { get; set; }

    }
}
