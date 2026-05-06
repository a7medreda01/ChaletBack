using AppBL.DTOs;
using AppBL.IService;
using AppBL.Mapper;
using AppBL.Service;
using AppDAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppPL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChaletController : ControllerBase
    {
        private readonly IChaletService _service;

        public ChaletController(IChaletService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ChaletCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    return Ok(await _service.GetAllAsync());
        //}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
                return NotFound(new { message = "الكوخ غير موجود" });

            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);

            if (!result)
                return NotFound(new { message = "الكوخ غير موجود" });

            return Ok(new { message = "تم حذف الكوخ بنجاح" });
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] ChaletUpdateDto dto)
        {
            try
            {
                var result = await _service.UpdateAsync(dto);

                return Ok(new
                {
                    success = true,
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

        [HttpGet]
        public async Task<IActionResult> GetChalets()
        {
            var userIdClaim = User.FindFirst("uid")?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("Invalid or missing user id");
            }

            try
            {
                if (User.IsInRole(Roles.Partner))
                {
                    var result = await _service.GetChaletsByPartnerAsync(userId);
                    return Ok(result);
                }

                var allResult = await _service.GetAllAsync();
                return Ok(allResult);
            }
            catch (Exception ex)
            {
                // مهم جدًا للتشخيص
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("by-type-period")]
        public async Task<IActionResult> GetAllChaletsByTypeAndPeriod(
    [FromQuery] ChaletType type,
    [FromQuery] BookingPeriod period)
        {
            var result = await _service
                .GetAllChaletsByTypeAndPeriodAsync(type, period);

            return Ok(result);
        }
    }
}
