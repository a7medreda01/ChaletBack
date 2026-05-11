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
    using AutoMapper;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;

    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepo;
        private readonly IHubContext<NotificationHub> _hub;
        private readonly IMapper _mapper;

        public NotificationService(
            INotificationRepository notificationRepo,
            IHubContext<NotificationHub> hub,
            IMapper mapper)
        {
            _notificationRepo = notificationRepo;
            _hub = hub;
            _mapper = mapper;
        }

        // ➕ Create Notification
        public async Task CreateAsync(string title, string message, int bookingId)
        {
            var notification = new Notification
            {
                Title = title,
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.UtcNow,
                BookingId = bookingId
            };

            await _notificationRepo.AddAsync(notification);
            await _notificationRepo.SaveAsync();

            var dto = _mapper.Map<NotificationDto>(notification);

            // 🔥 Send Real-time to Admin — CreatedAt بـ Z في الـ JSON
            await _hub.Clients.All.SendAsync("ReceiveNotification", dto);
        }

        // 📥 Get All
        public async Task<List<NotificationDto>> GetAllAsync()
        {
            var data = await _notificationRepo.GetAllAsync();
            return _mapper.Map<List<NotificationDto>>(data);
        }

        // 🔴 Unread only
        public async Task<List<NotificationDto>> GetUnreadAsync()
        {
            var data = await _notificationRepo.GetUnreadAsync();
            return _mapper.Map<List<NotificationDto>>(data);
        }

        // ✅ Mark as read
        public async Task MarkAsReadAsync(int id)
        {
            await _notificationRepo.MarkAsReadAsync(id);
            await _notificationRepo.SaveAsync();
        }
    }
}