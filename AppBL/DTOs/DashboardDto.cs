using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppBL.DTOs.BookingDTOs;


    public class DashboardDto
    {
        // Stats
        public int TotalBookings { get; set; }
        public int ConfirmedBookings { get; set; }
        public int DoneBookings { get; set; }
        public int PendingBookings { get; set; }
        public int CancelledBookings { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal ChaletRevenue { get; set; }
        public decimal ExtrasRevenue { get; set; }
        public decimal DepositSum { get; set; }
        public decimal DiscountSum { get; set; }

        // Previous period (للمقارنة)
        public int PrevTotalBookings { get; set; }
        public int PrevCancelledBookings { get; set; }
        public decimal PrevTotalRevenue { get; set; }

        // Recent bookings (آخر 10)
        public List<BookingDto> RecentBookings { get; set; } = [];

        // Chalets status
        public List<ChaletStatusDto> Chalets { get; set; } = [];
    }

    public class ChaletStatusDto
    {
        public string Name { get; set; } = "";
        public string Status { get; set; } = "";
        public string Type { get; set; } = "";
    }

