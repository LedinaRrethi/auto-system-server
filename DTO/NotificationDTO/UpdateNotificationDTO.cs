using System;
using System.ComponentModel.DataAnnotations;

namespace DTO.NotificationDTOs
{
    public class UpdateNotificationDTO
    {
        [Key]
        public Guid IDPK_Notification { get; set; }

        public bool IsSeen { get; set; } = false;
    }
}
