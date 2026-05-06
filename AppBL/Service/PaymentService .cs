using AppBL.DTOs;
using AppBL.IService;
using AppDAL.IRepo;

namespace AppBL.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepo;

        public PaymentService(IPaymentRepository paymentRepo)
        {
            _paymentRepo = paymentRepo;
        }

        public async Task<List<PaymentDto>> GetByBookingIdAsync(int bookingId)
        {
            var payments = await _paymentRepo.GetByBookingIdAsync(bookingId);

            return payments.Select(p => new PaymentDto
            {
                Id = p.Id,
                BookingId = p.BookingId,
                Amount = p.Amount,
                PaymentReson = p.PaymentReson,
                Method = p.Method.ToString(),
                Status = p.Status.ToString(),
                TransactionId = p.TransactionId,
                CreatedAt = p.CreatedAt
            }).ToList();
        }
        public async Task<object> GetTodayAndYesterdayPaymentsAsync()
        {
            var (today, yesterday) = await _paymentRepo.GetTodayAndYesterdayAsync();

            return new
            {
                Today = new
                {
                    Count = today.Count,
                    TotalAmount = today.Sum(x => x.Amount),
                    Payments = today
                },
                Yesterday = new
                {
                    Count = yesterday.Count,
                    TotalAmount = yesterday.Sum(x => x.Amount),
                    Payments = yesterday
                }
            };
        }
    }
}