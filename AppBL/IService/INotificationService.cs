using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppBL.DTOs;

namespace AppBL.IService
{
    public interface INotificationService
    {
        Task CreateAsync(string title, string message, int bookingId);
        Task<List<NotificationDto>> GetAllAsync();
        Task<List<NotificationDto>> GetUnreadAsync();
        Task MarkAsReadAsync(int id);
    }
}
