using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDAL.Entities
{
    public class Payment
    {
        public int Id { get; set; }

        public int BookingId { get; set; }
        public Booking Booking { get; set; }

        public decimal Amount { get; set; }
        public PaymentReson PaymentReson { get; set; } = PaymentReson.Deposit;
        //public string? UserName { get; set; } = null;
        public PaymentMethod Method { get; set; }
        public PaymentStatus Status { get; set; }

        public string TransactionId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
    public enum PaymentReson
    {
        Deposit,
        Price
    }
}
