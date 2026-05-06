using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppBL.DTOs.BookingDTOs;

namespace AppBL.IService
{
    public interface IBookingNoteService
    {
        Task AddNoteAsync(int bookingId, string note, string userName);
        Task<IEnumerable<BookingNoteDTO>> GetByBookingIdAsync(int bookingId);
    }
}
