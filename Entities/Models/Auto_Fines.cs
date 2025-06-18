using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;

namespace Entities.Models
{
    public partial class Auto_Fines
    {
        [Key]
        public Guid IDPK_Fine { get; set; }

        public Guid? IDFK_Vehicle { get; set; } 

        [Required]
        public Guid IDFK_FineRecipient { get; set; }

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
        public required string CreatedBy { get; set; } 

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [MaxLength(46)]
        public string? CreatedIp { get; set; }

        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        [MaxLength(46)]
        public string? ModifiedIp { get; set; }

        // Relationships
        [ForeignKey("IDFK_Vehicle")]
        public virtual Auto_Vehicles? Vehicle { get; set; }

        [ForeignKey("IDFK_FineRecipient")]
        public virtual Auto_FineRecipients FineRecipient { get; set; } = null!;

        [ForeignKey("CreatedBy")]
        public virtual Auto_Users PoliceOfficer { get; set; } = null!;

        [ForeignKey("ModifiedBy")]
        public virtual Auto_Users? ModifiedByUser { get; set; }
    }
}