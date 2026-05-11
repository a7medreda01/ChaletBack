using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppDAL.Entities;

namespace AppBL.IService
{

    using AppBL.DTOs;
    using AppBL.DTOs.BookingDTOs;

    public interface IBookingService
    {
        // =========================================
        // 🔍 Availability
        // =========================================
        Task<bool> CheckAvailability(int chaletId, DateTime date, BookingPeriod period);

        // =========================================
        // 🔥 Create Booking
        // =========================================
        Task<BookingResponseDto> CreateBooking(CreateBookingDto dto, int userId);

        // =========================================
        // 💰 Pricing
        // =========================================
        Task<(decimal chaletPrice, decimal extrasTotal, decimal total)> CalculatePriceDetails(
    ChaletType chaletType,
    BookingPeriod period,
    DateTime date,
    List<AddExtraTOBook> extrasDto = null);

        // =========================================
        // 💳 Confirm Booking
        // =========================================
        Task ConfirmBooking(int bookingId, decimal deposit);

        // =========================================
        // ❌ Cancel Booking
        // =========================================
        Task CancelBooking(int bookingId,string? notes,string UserName);

        // =========================================
        // 📊 Queries
        // =========================================
        Task<PagedResult<BookingDto>> GetBookingsPagedAsync(
            int? userId, bool isPartner,
            int page, int pageSize,
            string? search, string? status,
            string? dateFrom, string? dateTo);
        Task<BookingDto> GetDetails(int id);
        Task AddExtraToBooking(AddExtraDto dto);
        Task<BookingResponseDto> UpdateBookingAsync(UpdateBookingDto dto, string userName);
        Task<IEnumerable<BookingDto>> GetBookingsByPartnerAsync(int userId);
        Task<BookingResponseDto> MarkAsDone(int bookingId, int PayMoney, int chaletId, string UserName);
        Task<IEnumerable<BookingDto>> GetUpcomingBookingsAsync();
        Task<IEnumerable<BookingDto>> GetByTypeDatePeriodAsync(
    ChaletType chaletType,
    DateTime date,
    BookingPeriod period);
        Task<DashboardDto> GetDashboardAsync(
    int? userId, bool isPartner,
    DateTime from, DateTime to, DateTime prevFrom, DateTime prevTo);
        Task<List<BookingDto>> GetBookingsForExportAsync(
    int? userId, bool isPartner,
    int year, int month);

        List<CustomerDto> GetCustomersAsync();
    }
}
