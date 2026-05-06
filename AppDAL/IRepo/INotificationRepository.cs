using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppDAL.Entities;

namespace AppDAL.IRepo
{
public interface INotificationRepository
{
    Task AddAsync(Notification notification);
    Task<List<Notification>> GetAllAsync();
    Task<List<Notification>> GetUnreadAsync();
    Task MarkAsReadAsync(int id);
    Task SaveAsync();
}
}
