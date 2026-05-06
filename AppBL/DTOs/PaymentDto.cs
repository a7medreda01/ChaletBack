using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppDAL.Entities;

namespace AppBL.DTOs
{
    public class PaymentDto
    {
        public int Id { get; set; }

        public int BookingId { get; set; }

        public decimal Amount { get; set; }

        public string Method { get; set; }   // Cash / Card

        public string Status { get; set; }   // Pending / Paid / Failed

        public string TransactionId { get; set; }
        public PaymentReson PaymentReson { get; set; } = PaymentReson.Deposit;
        //public string? UserName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
