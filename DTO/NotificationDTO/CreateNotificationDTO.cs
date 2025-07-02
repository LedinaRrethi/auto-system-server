using Helpers.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.NotificationDTOs
{
    public record CreateNotificationDTO
    {
        [Required]
        public required string IDFK_Receiver { get; set; }

        [MaxLength(50)]
        public string? Title { get; set; }

        [MaxLength(1000)]
        public string? Message { get; set; }

        [Required]
        public NotificationType Type { get; set; }

        [Required]
        public required string CreatedBy { get; set; }



    }
}