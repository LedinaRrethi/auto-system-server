using DAL.UoW;
using Domain.Contracts;
using DTO.NotificationDTOs;
using Entities.Models;
using Microsoft.AspNetCore.Identity;

namespace Domain.Concrete
{
    public class NotificationDomain : INotificationDomain
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<Auto_Users> _userManager;

        public NotificationDomain(INotificationRepository notificationRepository, IUnitOfWork unitOfWork, UserManager<Auto_Users> userManager)
        {
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<List<NotificationDTO>> GetAllNotificationsAsync(string userId)
        {

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return new List<NotificationDTO>();

            var isIndivid = await _userManager.IsInRoleAsync(user, "Individ");
            if (!isIndivid) return new List<NotificationDTO>();

            var notifications = await _notificationRepository.GetAllNotificationsUserAsync(userId);

            return notifications.Select(n => new NotificationDTO
            {
                IDPK_Notification = n.IDPK_Notification,
                IDFK_Receiver = n.IDFK_Receiver,
                Title = n.Title,
                Message = n.Message,
                IsSeen = n.IsSeen,
                Type = n.Type,
                CreatedBy = n.CreatedBy,
                CreatedOn = n.CreatedOn
            }).ToList();
        }

        public async Task<List<NotificationDTO>> GetUnseenNotificationsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return new List<NotificationDTO>();

            var isIndivid = await _userManager.IsInRoleAsync(user, "Individ");
            if (!isIndivid) return new List<NotificationDTO>();

            var notifications = await _notificationRepository.GetAllNotificationsUnseenAsync(userId);

            return notifications.Select(n => new NotificationDTO
            {
                IDPK_Notification = n.IDPK_Notification,
                IDFK_Receiver = n.IDFK_Receiver,
                Title = n.Title,
                Message = n.Message,
                IsSeen = n.IsSeen,
                Type = n.Type,
                CreatedBy = n.CreatedBy,
                CreatedOn = n.CreatedOn
            }).ToList();
        }

        public async Task<int> CountUnseenNotificationsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null || !(await _userManager.IsInRoleAsync(user, "Individ")))
                return 0;

            return await _notificationRepository.CountUnseenNotificationsAsync(userId);
        }

        public async Task MarkAllNotificationsAsSeenAsync(string userId)
        {
            await _notificationRepository.MarkAllAsSeenAsync(userId);
            await _unitOfWork.CommitAsync();
        }

        public async Task<bool> MarkOneNotificationAsSeenAsync(Guid notificationId)
        {
            var updated = await _notificationRepository.MarkOneAsSeenAsync(notificationId);
            if (updated)
            {
                await _unitOfWork.CommitAsync();
            }
            return updated;
        }
    }
}