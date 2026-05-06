using AppBL.DTOs;

namespace AppBL.IService
{
    public interface IPaymentService
    {
        Task<List<PaymentDto>> GetByBookingIdAsync(int bookingId);
        Task<object> GetTodayAndYesterdayPaymentsAsync();

    }
}