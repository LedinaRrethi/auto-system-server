using DAL.Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Concrete
{
    public class NotificationRepository : BaseRepository<Auto_Notifications>, INotificationRepository
    {
        private readonly AutoSystemDbContext _context;

        public NotificationRepository(AutoSystemDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Auto_Notifications> GetNotificationsUser(string receiverId)
        {
            return _context.Auto_Notifications
                .Where(n => n.IDFK_Receiver == receiverId && n.Invalidated == 0)
                .OrderByDescending(n => n.CreatedOn)
                .ToList();
        }

        public IEnumerable<Auto_Notifications> GetNotificationsUnseen(string receiverId)
        {
            return _context.Auto_Notifications
                .Where(n => n.IDFK_Receiver == receiverId && !n.IsSeen && n.Invalidated == 0)
                .ToList();
        }

        public int CountUnreadNotifications(string receiverId)
        {
            return _context.Auto_Notifications
                .Count(n => n.IDFK_Receiver == receiverId && !n.IsSeen && n.Invalidated == 0);
        }

        public async Task AddNotificationAsync(Auto_Notifications notification)
        {
            await _context.Auto_Notifications.AddAsync(notification);
        }
    }
}
