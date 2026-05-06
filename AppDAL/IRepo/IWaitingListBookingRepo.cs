using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppDAL.Entities;

namespace AppDAL.IRepo
{
    public interface IWaitingListBookingRepo
    {
        Task<IEnumerable<WaitingList>> GetAllBookingsAsync();

    }
}
