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
    public interface IBookingExtraRepository : IGenericRepository<BookingExtra>
    {
        Task<IEnumerable<BookingExtra>> GetByBookingId(int bookingId);
    }

    public class BookingExtraRepository : GenericRepository<BookingExtra>, IBookingExtraRepository
    {
        private readonly HotelDbContext _context;

        public BookingExtraRepository(HotelDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BookingExtra>> GetByBookingId(int bookingId)
        {
            return await _context.BookingExtras.Include(b=>b.Extra)
                .Where(b => b.BookingId == bookingId)
                .ToListAsync();
        }
    }
}
