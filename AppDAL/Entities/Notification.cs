using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDAL.Entities
{
    public class Notification
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public bool IsRead { get; set; } = false;

        public int? UserId { get; set; } // لو إشعار لمستخدم معين (Admin)
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int?  BookingId { get; set; }
    }
}
