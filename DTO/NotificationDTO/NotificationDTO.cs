using Helpers.Enumerations;
using System;
using System.ComponentModel.DataAnnotations;

namespace DTO.NotificationDTOs
{
    public class NotificationDTO
    {
        [Key]
        public Guid IDPK_Notification { get; set; }

        [Required]
        public required string IDFK_Receiver { get; set; }

        [MaxLength(50)]
        public string? Title { get; set; }

        [MaxLength(1000)]
        public string? Message { get; set; }

        public bool IsSeen { get; set; } = false;

        [Required]
        public NotificationType Type { get; set; }

        [Required]
        public required string CreatedBy { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    }
}
