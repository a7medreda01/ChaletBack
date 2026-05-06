using AppDAL.Context;
using AppDAL.Entities;
using AppDAL.IRepo;
using Microsoft.EntityFrameworkCore;

namespace AppDAL.Repo
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        private readonly HotelDbContext _context;

        public PaymentRepository(HotelDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Payment>> GetByBookingIdAsync(int bookingId)
        {
            return await _context.Payments
                .Where(p => p.BookingId == bookingId)
                .ToListAsync();
        }
        public async Task<(List<Payment> today, List<Payment> yesterday)> GetTodayAndYesterdayAsync()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            var yesterday = today.AddDays(-1);

            var todayPayments = await _context.Payments
                .Where(p => p.CreatedAt >= today && p.CreatedAt < tomorrow)
                .ToListAsync();

            var yesterdayPayments = await _context.Payments
                .Where(p => p.CreatedAt >= yesterday && p.CreatedAt < today)
                .ToListAsync();

            return (todayPayments, yesterdayPayments);
        }
    }
}