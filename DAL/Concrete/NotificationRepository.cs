using DAL.Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Concrete
{
    public class NotificationRepository : BaseRepository<Auto_Notifications>, INotificationRepository
    {
        public NotificationRepository(AutoSystemDbContext context) : base(context) { }

        public async Task<List<Auto_Notifications>> GetAllNotificationsUserAsync(string receiverId)
        {
            return await _context.Auto_Notifications
                .Where(n => n.IDFK_Receiver == receiverId && n.Invalidated == 0)
                .OrderByDescending(n => n.CreatedOn)
                .ToListAsync();
        }

        public async Task<List<Auto_Notifications>> GetAllNotificationsUnseenAsync(string receiverId)
        {
            return await _context.Auto_Notifications
                .Where(n => n.IDFK_Receiver == receiverId && !n.IsSeen && n.Invalidated == 0)
                .ToListAsync();
        }

        public async Task<int> CountUnseenNotificationsAsync(string receiverId)
        {
            return await _context.Auto_Notifications
                .CountAsync(n => n.IDFK_Receiver == receiverId && !n.IsSeen && n.Invalidated == 0);
        }

        public async Task AddNotificationAsync(Auto_Notifications notification)
        {
            await _context.Auto_Notifications.AddAsync(notification);
        }

        public async Task MarkAllAsSeenAsync(string receiverId)
        {
            await _context.Database.ExecuteSqlRawAsync(@"
                UPDATE Auto_Notifications
                SET IsSeen = 1
                WHERE IDFK_Receiver = {0} AND IsSeen = 0 AND Invalidated = 0", receiverId);
        }

        public async Task<bool> MarkOneAsSeenAsync(Guid notificationId)
        {
            var notification = await _context.Auto_Notifications
                .FirstOrDefaultAsync(n => n.IDPK_Notification == notificationId && n.Invalidated == 0);

            if (notification != null)
            {
                notification.IsSeen = true;
                return true;
            }

            return false;
        }
    }
}
