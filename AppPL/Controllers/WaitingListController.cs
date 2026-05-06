using AppBL.DTOs;
using AppBL.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppPL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WaitingListController : ControllerBase
    {
        private readonly IWaitingListService _service;

        public WaitingListController(IWaitingListService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpPost("convert-to-booking/{id}")]
        public async Task<IActionResult> ConvertToBooking(int id)
        {
            var result = await _service.ConvertWaitingToBookingAsync(id);

            //if (!result.Success)
            //    return BadRequest(result.Message);

            return Ok(new
            {
                message = result
            });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWaiting(int id, UpdateWaitingListDto dto)
        {
            await _service.UpdateWaitingAsync(id, dto);
            return Ok(new
            {
                message = "تم التحديث بنجاح"
            });
        }
    }
}
