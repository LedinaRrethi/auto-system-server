using Entities.Models;
using Helpers.Enumerations;
using System;
using System.Collections.Generic;

namespace DAL.Contracts
{
    public interface INotificationRepository : IRepository<Auto_Notifications>
    {
        IEnumerable<Auto_Notifications> GetNotificationsUser(string receiverId);
        IEnumerable<Auto_Notifications> GetNotificationsUnseen(string receiverId);
        int CountUnreadNotifications(string receiverId);

        Task AddNotificationAsync(Auto_Notifications notification);
    }
}
