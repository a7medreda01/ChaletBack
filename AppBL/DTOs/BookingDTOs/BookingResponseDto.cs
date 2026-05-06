using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBL.DTOs.BookingDTOs
{
    public class BookingResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public int? BookingId { get; set; }

        // 💰 Useful للـ UI
        public decimal? TotalPrice { get; set; }

        // ⏱ لو Pending
        public DateTime? ExpireAt { get; set; }

        // 🔄 Status
        public string Status { get; set; }
        public string? Notes { get; set; }

    }
}
