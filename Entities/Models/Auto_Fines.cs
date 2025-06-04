using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Auto_Fines
    {
        [Key]
        public Guid IDPK_Fine { get; set; }

        public Guid? IDFK_Vehicle { get; set; } // Nëse makina është e regjistruar

        [Required]
        public Guid IDFK_FineRecipient { get; set; } // Lidhet me Auto_FineRecipients

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal FineAmount { get; set; }

        [Required]
        public DateTime FineDate { get; set; }

        [MaxLength(500)]
        public string? FineReason { get; set; }

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
        [ForeignKey("IDFK_Vehicle")]
        public virtual Auto_Vehicles? Vehicle { get; set; }

        [ForeignKey("IDFK_FineRecipient")]
        public virtual Auto_FineRecipients FineRecipient { get; set; } = null!;

        [ForeignKey("CreatedBy")]
        public virtual Auto_Users CreatedByUser { get; set; } = null!;

        [ForeignKey("ModifiedBy")]
        public virtual Auto_Users? ModifiedByUser { get; set; }
    }
}
