using AppBL.DTOs;
using AppBL.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppPL.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class HolidayController : ControllerBase
    {
        private readonly IHolidayService _service;

        public HolidayController(IHolidayService service)
        {
            _service = service;
        }

        // =========================================
        // 📄 Get All
        // =========================================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAll();
            return Ok(data);
        }

        // =========================================
        // 📄 Get By Id
        // =========================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _service.GetById(id);
            return Ok(data);
        }

        // =========================================
        // ➕ Add
        // =========================================
        [HttpPost]
        public async Task<IActionResult> Add(CreateHolidayDto dto)
        {
            await _service.Add(dto);
            return Ok("تم إضافة العطلة");
        }

        // =========================================
        // ✏ Update
        // =========================================
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateHolidayDto dto)
        {
            await _service.Update(id, dto);
            return Ok("تم التعديل");
        }

        // =========================================
        // ❌ Delete
        // =========================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.Delete(id);
            return Ok("تم الحذف");
        }

        // =========================================
        // 🔍 Check if Holiday
        // =========================================
        [HttpGet("check")]
        public async Task<IActionResult> Check(DateTime date)
        {
            var result = await _service.IsHoliday(date);

            return Ok(new
            {
                isHoliday = result
            });
        }
    }
}
