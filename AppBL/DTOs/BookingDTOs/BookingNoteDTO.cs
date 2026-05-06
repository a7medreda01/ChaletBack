using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBL.DTOs.BookingDTOs
{
    public class BookingNoteDTO
    {
        public int Id { get; set; }

        public int BookingId { get; set; }

        public string Note { get; set; }

        public string UserName { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
