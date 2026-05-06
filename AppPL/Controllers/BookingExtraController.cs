using AppBL.IService;
using Microsoft.AspNetCore.Mvc;

namespace AppPL.Controllers
{
    using AppBL.DTOs;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class BookingExtraController : ControllerBase
    {
        private readonly IBookingExtraService _service;

        public BookingExtraController(IBookingExtraService service)
        {
            _service = service;
        }

        [HttpGet("booking/{bookingId}")]
        public async Task<IActionResult> GetByBooking(int bookingId)
        {
            var result = await _service.GetByBookingId(bookingId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookingExtraDto dto)
        {
            var result = await _service.AddAsync(dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
    }
}
