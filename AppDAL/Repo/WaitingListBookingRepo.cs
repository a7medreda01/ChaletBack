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
    public class WaitingListBookingRepo : IWaitingListBookingRepo
    {
        private readonly HotelDbContext _context;

        public WaitingListBookingRepo(HotelDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<WaitingList>> GetAllBookingsAsync()
        {
            return await _context.WaitingLists.Include(w => w.Chalet).ToListAsync();
        }
    }
}
