using AppDAL.Entities;
using System.Linq.Expressions;

namespace AppDAL.IRepo
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<List<Payment>> GetByBookingIdAsync(int bookingId);
        Task<(List<Payment> today, List<Payment> yesterday)> GetTodayAndYesterdayAsync();

    }
}