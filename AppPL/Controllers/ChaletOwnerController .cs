using AppBL.DTOs.ChaletOwnerDTO;
using AppBL.IService;
using AppBL.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppPL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChaletOwnerController : ControllerBase
    {
        private readonly IChaletOwnerService _service;

        public ChaletOwnerController(IChaletOwnerService service)
        {
            _service = service;
        }

        // ➕ إضافة شريك لشاليه
        [HttpPost]
        public async Task<IActionResult> AddOwner(AddChaletOwnerDto dto)
        {
            var result = await _service.AddOwnerAsync(dto);
            return Ok(result);
        }
        //[HttpGet]
        //public async Task<IActionResult> GetChaletsByPartnerAsync()
        //{
        //    var result = await _service.GetChaletsByPartnerAsync();
        //    return Ok(result);
        //}

        // 📄 عرض شركاء شاليه
        //[HttpGet("chalet/{chaletId}")]
        //public async Task<IActionResult> GetOwners(int chaletId)
        //{
        //    var result = await _service.GetOwnersByChalet(chaletId);
        //    return Ok(result);
        //}

        // 💰 توزيع أرباح
        //[HttpPost("distribute/{bookingId}")]
        //public async Task<IActionResult> DistributeProfit(int bookingId)
        //{
        //    await _service.DistributeProfitAsync(bookingId);
        //    return Ok(new { message = "تم توزيع الأرباح" });
        //}

        [HttpGet("chalets-with-partners")]
        public async Task<IActionResult> GetChaletsWithPartners()
        {
            var data = await _service.GetChaletsWithPartnersAsync();
            return Ok(data);
        }
    }  
}
