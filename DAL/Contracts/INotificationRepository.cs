using Entities.Models;

public interface INotificationRepository
{
    Task<List<Auto_Notifications>> GetAllNotificationsUserAsync(string receiverId);
    Task<List<Auto_Notifications>> GetAllNotificationsUnseenAsync(string receiverId);
    Task<int> CountUnseenNotificationsAsync(string receiverId);
    Task AddNotificationAsync(Auto_Notifications notification);
    Task MarkAllAsSeenAsync(string receiverId);
    Task<bool> MarkOneAsSeenAsync(Guid notificationId);
}
