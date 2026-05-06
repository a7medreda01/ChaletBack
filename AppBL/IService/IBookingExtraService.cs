using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppBL.DTOs;
using AppBL.DTOs.BookingDTOs;
using AppDAL.Repo;

namespace AppBL.IService
{
    public interface IBookingExtraService
    {
        Task<IEnumerable<BookingExtraDto>> GetByBookingId(int bookingId);
        Task<BookingExtraDto> AddAsync(CreateBookingExtraDto dto);
        Task DeleteAsync(int id);
    }
}
