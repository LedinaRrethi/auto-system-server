using DTO.NotificationDTOs;

namespace Domain.Contracts
{
    public interface INotificationDomain
    {
        Task<List<NotificationDTO>> GetAllNotificationsAsync(string userId);
        Task<List<NotificationDTO>> GetUnseenNotificationsAsync(string userId);
        Task<int> CountUnseenNotificationsAsync(string userId);
        Task MarkAllNotificationsAsSeenAsync(string userId);
        Task MarkOneNotificationAsSeenAsync(Guid notificationId);
    }
}
