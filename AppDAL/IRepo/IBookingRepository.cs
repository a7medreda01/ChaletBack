using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppDAL.Entities;

namespace AppDAL.IRepo
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        // 🔍 Check Availability
        Task<bool> ExistsAsync(int chaletId, DateTime date, BookingPeriod period);

        // 📅 Get bookings by date
        Task<IEnumerable<Booking>> GetByDateAsync(DateTime date);

        // 🏡 Get bookings for chalet
        Task<IEnumerable<Booking>> GetByChaletAsync(int chaletId);

        // 📊 Get full booking details
        Task<Booking?> GetWithDetailsAsync(int bookingId);

        // ❌ Cancel booking
        Task CancelAsync(int bookingId);

        // 🔎 Get active bookings (for dashboard)
        // IBookingRepository.cs
        Task<(List<Booking> Items, int Total)> GetBookingsPagedAsync(
            int? userId, bool isPartner,
            int page, int pageSize,
            string? search, string? status,
            string? dateFrom, string? dateTo);
        // get book
        Task<Booking?> GetByChaletDatePeriodAsync(int chaletId, DateTime date, int period);
        //Task<List<Booking>> GetAllByChaletDatePeriodAsync(int chaletId, DateTime date, int period);
        Task<List<Booking>> GetBookingsByPartnerAsync(int userId);

        //الاكواخ المتاحه
        Task<List<Chalet>> RepoGetChaletsByTypeAndPeriodAsync(ChaletType ChaletType, BookingPeriod Period);
        //المتاح لدخول العميل
        Task<List<Booking>> GetByTypeDatePeriodAsync(ChaletType chaletType, DateTime date, BookingPeriod period);
        Task<List<Booking>> GetUpcomingBookingsAsync();
        List<(string Phone, string CustomerName, int Count, DateTime LastDate)> GetCustomersRawAsync();




    }
}
