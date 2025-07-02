using Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Contracts
{
    public interface INotificationRepository : IRepository<Auto_Notifications>
    {
        IEnumerable<Auto_Notifications> GetAllNotificationsUser(string receiverId);
        IEnumerable<Auto_Notifications> GetAllNotificationsUnseen(string receiverId);
        int CountUnseenNotifications(string receiverId);

        Task AddNotificationAsync(Auto_Notifications notification);

        Task MarkAllAsSeenAsync(string receiverId);
        Task<bool> MarkOneAsSeenAsync(Guid notificationId);
        Task SaveChangesAsync();
    }
}
