using AppBL.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppPL.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _service;

        public NotificationsController(INotificationService service)
        {
            _service = service;
        }

        // 📥 كل الإشعارات
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(data);
        }

        // 🔴 غير المقروء
        [HttpGet("unread")]
        public async Task<IActionResult> GetUnread()
        {
            var data = await _service.GetUnreadAsync();
            return Ok(data);
        }

        // 🔢 عدد غير المقروء
        //[HttpGet("count")]
        //public async Task<IActionResult> Count()
        //{
        //    var count = await _service.GetUnreadCountAsync();
        //    return Ok(count);
        //}

        // ✅ mark as read
        [HttpPost("read/{id}")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            await _service.MarkAsReadAsync(id);
            return Ok();
        }
    }
}
