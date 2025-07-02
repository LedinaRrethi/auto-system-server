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

        public IEnumerable<Auto_Notifications> GetAllNotificationsUser(string receiverId)
        {
            return _context.Auto_Notifications
                .Where(n => n.IDFK_Receiver == receiverId && n.Invalidated == 0)
                .OrderByDescending(n => n.CreatedOn)
                .ToList();
        }

        public IEnumerable<Auto_Notifications> GetAllNotificationsUnseen(string receiverId)
        {
            return _context.Auto_Notifications
                .Where(n => n.IDFK_Receiver == receiverId && !n.IsSeen && n.Invalidated == 0)
                .ToList();
        }

        public int CountUnseenNotifications(string receiverId)
        {
            return _context.Auto_Notifications
                .Count(n => n.IDFK_Receiver == receiverId && !n.IsSeen && n.Invalidated == 0);
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

                //i tregon ef qe ky entitet esht ndryshuar 
                //_context.Entry(notification).Property(n => n.IsSeen).IsModified = true;
                return true;
            }

            return false;
        }


        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
