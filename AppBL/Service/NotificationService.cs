using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBL.Service
{
    using AppBL.DTOs;
    using AppBL.IService;
    using AppDAL.Context;
    using AppDAL.Entities;
    using AppDAL.IRepo;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;

    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepo;
        private readonly IHubContext<NotificationHub> _hub;

        public NotificationService(
            INotificationRepository notificationRepo,
            IHubContext<NotificationHub> hub)
        {
            _notificationRepo = notificationRepo;
            _hub = hub;
        }

        // ➕ Create Notification
        public async Task CreateAsync(string title, string message, int bookingId)
        {
            var notification = new Notification
            {
                Title = title,
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.Now,
                BookingId=bookingId
            };

            await _notificationRepo.AddAsync(notification);
            await _notificationRepo.SaveAsync();

            // 🔥 Send Real-time to Admin
            await _hub.Clients.All.SendAsync("ReceiveNotification", new
            {
                notification.Id,
                notification.Title,
                notification.Message,
                notification.CreatedAt,
                notification.BookingId
            });
        }

        // 📥 Get All
        public async Task<List<NotificationDto>> GetAllAsync()
        {
            var data = await _notificationRepo.GetAllAsync();

            return data.Select(x => new NotificationDto
            {
                Id = x.Id,
                Title = x.Title,
                Message = x.Message,
                IsRead = x.IsRead,
                CreatedAt = x.CreatedAt,
                BookingId=x.BookingId
            }).ToList();
        }

        // 🔴 Unread only
        public async Task<List<NotificationDto>> GetUnreadAsync()
        {
            var data = await _notificationRepo.GetUnreadAsync();

            return data.Select(x => new NotificationDto
            {
                Id = x.Id,
                Title = x.Title,
                Message = x.Message,
                IsRead = x.IsRead,
                CreatedAt = x.CreatedAt,
                BookingId= x.BookingId
            }).ToList();
        }


        // ✅ Mark as read
        public async Task MarkAsReadAsync(int id)
        {
            await _notificationRepo.MarkAsReadAsync(id);
            await _notificationRepo.SaveAsync();
        }
    }
}
