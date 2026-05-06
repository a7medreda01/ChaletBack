using System.Security.Claims;
using AppBL.DTOs.BookingDTOs;
using AppBL.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppPL.Controllers
{
    [ApiController]
    [Route("api/booking-notes")]
    public class BookingNoteController : ControllerBase
    {
        private readonly IBookingNoteService _noteService;

        public BookingNoteController(IBookingNoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpPost]
        public async Task<IActionResult> AddNote(CreateBookingNoteDTO dto)
        {
            var employeeName = User.FindFirst(ClaimTypes.Name)?.Value
                              ?? User.Identity?.Name
                              ?? "Unknown";

            await _noteService.AddNoteAsync(dto.BookingId, dto.Note, employeeName);

            return Ok(new { message = "تم إضافة الملاحظة" });
        }

        [HttpGet("{bookingId}")]
        public async Task<IActionResult> GetNotes(int bookingId)
        {
            var notes = await _noteService.GetByBookingIdAsync(bookingId);
            return Ok(notes);
        }
    }
}
