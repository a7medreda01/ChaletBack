using AppBL.IService;
using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("by-booking/{bookingId}")]
        public async Task<IActionResult> GetByBookingId(int bookingId)
        {
            var result = await _paymentService.GetByBookingIdAsync(bookingId);
            return Ok(result);
        }
        [HttpGet("summary")]
        public async Task<IActionResult> GetPaymentsSummary()
        {
            var result = await _paymentService.GetTodayAndYesterdayPaymentsAsync();
            return Ok(result);
        }
    }
}