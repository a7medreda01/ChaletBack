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
        Task<IEnumerable<BookingDto>> GetAll();
        Task<BookingDto> GetDetails(int id);
        Task AddExtraToBooking(AddExtraDto dto);
        Task<BookingResponseDto> UpdateBookingAsync(UpdateBookingDto dto);
        Task<IEnumerable<BookingDto>> GetBookingsByPartnerAsync(int userId);
        Task<BookingResponseDto> MarkAsDone(int bookingId, int PayMoney, int chaletId);
        Task<IEnumerable<BookingDto>> GetUpcomingBookingsAsync();
        Task<IEnumerable<BookingDto>> GetByTypeDatePeriodAsync(
    ChaletType chaletType,
    DateTime date,
    BookingPeriod period);


    }
}
