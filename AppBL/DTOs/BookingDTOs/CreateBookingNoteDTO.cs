using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBL.DTOs.BookingDTOs
{
    public class CreateBookingNoteDTO
    {
        public int BookingId { get; set; }

        public string? Note { get; set; }

        public DateTime CreatedAt { get; set; }=DateTime.UtcNow;
    }
}
