using Entities.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class AutoNotification
{
    [Key]
    public Guid IDPK_Notification { get; set; }

    [Required]
    public Guid FKID_Receiver { get; set; }

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
    public Guid CreatedBy { get; set; }

    [Required]
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    [MaxLength(46)]
    public string? CreatedIp { get; set; }

    public Guid? ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }

    [MaxLength(46)]
    public string? ModifiedIp { get; set; }

    // Relationships
    [ForeignKey("FKID_Receiver")]
    public virtual Auto_Users Receiver { get; set; } = null!;

    [ForeignKey("CreatedBy")]
    public virtual Auto_Users Sender { get; set; } = null!;

    [ForeignKey("ModifiedBy")]
    public virtual Auto_Users? ModifiedByUser { get; set; }
}

public enum NotificationType : byte
{
    FineIssued = 0,
    InspectionResult = 1,
    General = 2
}
