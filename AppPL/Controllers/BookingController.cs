using System.Security.Claims;
using AppBL.DTOs;
using AppBL.DTOs.BookingDTOs;
using AppBL.IService;
using AppBL.Mapper;
using AppBL.Service;
using AppDAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppPL.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _service;

        public BookingController(IBookingService service)
        {
            _service = service;
        }

        // =========================================
        // 🔍 Check Availability
        // =========================================
        [HttpGet("check")]
        public async Task<IActionResult> CheckAvailability(
            int chaletId,
            DateTime date,
            BookingPeriod period)
        {
            var available = await _service.CheckAvailability(chaletId, date, period);

            return Ok(new
            {
                available,
                message = available ? "متاح" : "غير متاح"
            });
        }

        // =========================================
        // 🔥 Create Booking
        // =========================================
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookingDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userId = int.Parse(User.FindFirst("uid")?.Value);
            var UserName = User.FindFirst(ClaimTypes.Name)?.Value
                               ?? User.Identity?.Name
                               ?? "Unknown";
            dto.UserName = UserName;
            var result = await _service.CreateBooking(dto, userId);

            return Ok(new
            {
                message = result
            });
        }

        // =========================================
        // 💳 Confirm Booking (Manager Only)
        // =========================================
        [Authorize(Roles = Roles.Manager)]
        [HttpPost("{id}/confirm")]
        public async Task<IActionResult> Confirm(int id, [FromQuery] decimal deposit)
        {
            await _service.ConfirmBooking(id, deposit);

            return Ok(new
            {
                message = "تم تأكيد الحجز بنجاح"
            });
        }

        // =========================================
        // ❌ Cancel Booking
        // =========================================
        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id,[FromBody] CancelBookingDto dto)
        {
            // اسم الموظف من التوكن
            // اسم الموظف من التوكن
            var employeeName = User.FindFirst(ClaimTypes.Name)?.Value
                               ?? User.Identity?.Name
                               ?? "Unknown";

            // دمج النص
            //var notes = $"تم الإلغاء بواسطة {employeeName} - السبب: {dto.Reason}";
            await _service.CancelBooking(id, dto.Reason,employeeName);

            return Ok(new
            {
                message = "تم إلغاء الحجز"
            });
        }

        // =========================================
        // 📊 Get All Active Bookings
        // =========================================
        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    var data = await _service.GetAll();

        //    return Ok(data);
        //}



        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetBooks(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? search = null,
            [FromQuery] string? status = null,
            [FromQuery] string? dateFrom = null,
            [FromQuery] string? dateTo = null)
        {
            var userIdClaim = User.FindFirst("uid")?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized("Invalid or missing user id");

            try
            {
                var isPartner = User.IsInRole(Roles.Partner);
                var result = await _service.GetBookingsPagedAsync(
                    userId, isPartner, page, pageSize, search, status, dateFrom, dateTo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        // =========================================
        // 📄 Get Booking Details
        // =========================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetails(int id)
        {
            var booking = await _service.GetDetails(id);

            return Ok(booking);
        }

        [HttpPost("{bookingId}/extras")]
        public async Task<IActionResult> AddExtra(AddExtraDto dto)
        {
            await _service.AddExtraToBooking(dto);
            return Ok(new
            {
                success = true,
                message = "تمت إضافة الإضافة بنجاح"
            });
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateBooking([FromBody] UpdateBookingDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var UserName = User.FindFirst(ClaimTypes.Name)?.Value
                   ?? User.Identity?.Name
                   ?? "Unknown";
            try
            {
                var result = await _service.UpdateBookingAsync(dto, UserName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }
        [Authorize]
        [HttpPut("{id}/done")]
        public async Task<IActionResult> MarkAsDone(int id, int PayMoney, int chaletId)
        {
            // اسم الموظف من التوكن
            var employeeName = User.FindFirst(ClaimTypes.Name)?.Value
                               ?? User.Identity?.Name
                               ?? "Unknown";

            try
            {
                var result = await _service.MarkAsDone(id, PayMoney,chaletId, employeeName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BookingResponseDto
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
        [HttpGet("upcoming")]
        public async Task<IActionResult> GetUpcomingBookings()
        {
            try
            {
                var result = await _service.GetUpcomingBookingsAsync();

                return Ok(new
                {
                    success = true,
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
        [HttpGet("by-type-date-period")]
        public async Task<IActionResult> GetByTypeDatePeriod(
    ChaletType chaletType,
    DateTime date,
    BookingPeriod period)
        {
            try
            {
                var result = await _service
                    .GetByTypeDatePeriodAsync(chaletType, date, period);

                return Ok(new
                {
                    success = true,
                    count = result.Count(),
                    data = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }


        [Authorize]
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard(
    [FromQuery] string filter = "30",
    [FromQuery] string? dateFrom = null,
    [FromQuery] string? dateTo = null)
        {
            var userIdClaim = User.FindFirst("uid")?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            var isPartner = User.IsInRole(Roles.Partner);
            var now = DateTime.Today;
            DateTime from, to, prevFrom, prevTo;

            if (filter == "custom" && dateFrom != null && dateTo != null)
            {
                from = DateTime.Parse(dateFrom);
                to = DateTime.Parse(dateTo);
                var diff = to - from;
                prevTo = from.AddDays(-1);
                prevFrom = prevTo - diff;
            }
            else if (filter == "month")
            {
                from = new DateTime(now.Year, now.Month, 1);
                to = now;
                prevFrom = from.AddMonths(-1);
                prevTo = from.AddDays(-1);
            }
            else
            {
                var days = int.TryParse(filter, out var d) ? d : 30;
                from = now.AddDays(-days);
                to = now;
                prevFrom = now.AddDays(-days * 2);
                prevTo = from.AddDays(-1);
            }

            var result = await _service.GetDashboardAsync(
                userId, isPartner, from, to, prevFrom, prevTo);

            return Ok(result);
        }
        [Authorize]
        [HttpGet("export")]
        public async Task<IActionResult> ExportBookings(
    [FromQuery] int year,
    [FromQuery] int month)
        {
            var userIdClaim = User.FindFirst("uid")?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            var isPartner = User.IsInRole(Roles.Partner);
            var result = await _service.GetBookingsForExportAsync(userId, isPartner, year, month);
            return Ok(result);
        }

        [HttpGet("customers")]
        public IActionResult GetAll()
        {
            var result =  _service.GetCustomersAsync();
            return Ok(result);
        }
    }
}
