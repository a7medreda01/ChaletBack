using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppDAL.Context;
using AppDAL.Entities;
using AppDAL.IRepo;
using Microsoft.EntityFrameworkCore;

namespace AppDAL.Repo
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        public BookingRepository(HotelDbContext context) : base(context)
        {
        }

        // 🔍 Check if booking exists
        public async Task<bool> ExistsAsync(int chaletId, DateTime date, BookingPeriod period)
        {
            return await _context.Bookings.AnyAsync(b =>
                b.ChaletId == chaletId &&
                b.Date.Date == date.Date &&
                b.Period == period &&
                b.Status != BookingStatus.Cancelled);
        }

        // 📅 Get bookings by date
        public async Task<IEnumerable<Booking>> GetByDateAsync(DateTime date)
        {
            return await _context.Bookings
                .Where(b => b.Date.Date == date.Date)
                .Include(b => b.Chalet)
                .ToListAsync();
        }

        // 🏡 Get bookings for chalet
        public async Task<IEnumerable<Booking>> GetByChaletAsync(int chaletId)
        {
            return await _context.Bookings
                .Where(b => b.ChaletId == chaletId)
                .ToListAsync();
        }

        // 📊 Full details
        public async Task<Booking?> GetWithDetailsAsync(int bookingId)
        {
            return await _context.Bookings
                .Include(b => b.Chalet)
                .Include(b => b.BookingExtras)
                    .ThenInclude(be => be.Extra)
                .Include(b => b.CreatedByUser)
                .Include(b => b.Payments)
                .Include(b => b.Notes)
                .FirstOrDefaultAsync(b => b.Id == bookingId);
        }

        // ❌ Cancel
        public async Task CancelAsync(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking != null)
            {
                booking.Status = BookingStatus.Cancelled;
            }
        }

        // 📊 Active bookings
        public async Task<IEnumerable<Booking>> GetBookingsAsync()
        {
            return await _context.Bookings.Include(b=>b.Chalet).Include(b=>b.BookingExtras).ThenInclude(b=>b.Extra).Include(b=>b.CreatedByUser).Include(b=>b.Payments)
                .Include(b=>b.Notes)
                .ToListAsync();
        }

        //الحجز الحالي
        public async Task<Booking?> GetByChaletDatePeriodAsync(int chaletId, DateTime date, int period)
        {
            return await _context.Bookings
                .FirstOrDefaultAsync(b =>
                    b.ChaletId == chaletId &&
                    b.Date.Date == date.Date &&
                    (int)b.Period == period);
        }

        public async Task<List<Booking>> GetAllByChaletDatePeriodAsync(int chaletId, DateTime date, int period)
        {
            var targetDate = date.Date;

            return await _context.Bookings
                .Where(b =>
                    b.ChaletId == chaletId &&
                    b.Date >= targetDate &&
                    b.Date < targetDate.AddDays(1) &&
                    (int)b.Period == period
                )
                .ToListAsync();
        }
        public async Task<List<Booking>> GetBookingsByPartnerAsync(int userId)
        {
            return await _context.Bookings
                .Where(b => b.Chalet.ChaletOwners.Any(o => o.UserId == userId))
                .Include(b => b.Chalet).Include(b => b.CreatedByUser).Include(b => b.Payments).Include(b => b.Notes)
                .AsNoTracking()
                .ToListAsync();
        }



        //شغل جديد مهم

        //الاسنتخدام وقت التأكيد
        public  async Task<List<Booking>> GetByTypeDatePeriodAsync(ChaletType chaletType, DateTime date, BookingPeriod period)
        {
            var targetDate = date.Date;

            return await _context.Bookings
                .Where(b =>
                    b.ChaletType == chaletType &&
                    b.Date >= targetDate &&
                    b.Date < targetDate.AddDays(1) &&
                    b.Period == period && b.Status !=BookingStatus.Cancelled
                )
                .ToListAsync();
        }

        //الاستهدام وقت العرض 
        public async Task<List<Chalet>> RepoGetChaletsByTypeAndPeriodAsync(
            ChaletType type,
            BookingPeriod period)
        {
            return await _context.Chalets
                .Where(c => c.Type == type)
                .Where(c =>
                    (period == BookingPeriod.Morning && c.HasMorning) ||
                    (period == BookingPeriod.Evening && c.HasEvening) ||
                    (period == BookingPeriod.Full && c.HasFullDay)
                )
                .ToListAsync();
        }
        // الاستخدام لعرض الايام المتاحه حسب النوع والفترة في الفرونت
        public async Task<List<Booking>> GetUpcomingBookingsAsync()
        {
            var today = DateTime.Today;

            return await _context.Bookings
                .Where(b => b.Date >= today)
                .Where(b => b.Status != BookingStatus.Cancelled)
                .OrderBy(b => b.Date)
                .ToListAsync();
        }


    }
}
