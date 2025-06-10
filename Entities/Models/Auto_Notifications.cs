using Entities.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Helpers.Enumerations;

namespace Entities.Models
{
    public partial class Auto_Notifications
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


        // Audit Fields
        public byte Invalidated { get; set; } = 0;

        [Required]
        public required string CreatedBy { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [MaxLength(46)]
        public string? CreatedIp { get; set; }

        // Relationships
        [ForeignKey("IDFK_Receiver")]
        public virtual Auto_Users Receiver { get; set; } = null!;

        [ForeignKey("CreatedBy")]
        public virtual Auto_Users Sender { get; set; } = null!;

    }

}