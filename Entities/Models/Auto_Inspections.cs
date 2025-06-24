using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public partial class Auto_Inspections
    {
        [Key]
        public Guid IDPK_Inspection { get; set; }

        [Required]
        public Guid IDFK_InspectionRequest { get; set; }

        public string? IDFK_Specialist { get; set; }

        public bool IsPassed { get; set; } = false;

        public string? Comment { get; set; }

        public byte Invalidated { get; set; } = 0;

        [Required]
        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [MaxLength(46)]
        public string? CreatedIp { get; set; }

        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        [MaxLength(46)]
        public string? ModifiedIp { get; set; }

        [ForeignKey(nameof(IDFK_InspectionRequest))]
        public virtual Auto_InspectionRequests Request { get; set; } = null!;

        [ForeignKey(nameof(IDFK_Specialist))]
        public virtual Auto_Users? Specialist { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        public virtual Auto_Users CreatedByUser { get; set; } = null!;

        [ForeignKey(nameof(ModifiedBy))]
        public virtual Auto_Users? ModifiedByUser { get; set; }

        public virtual ICollection<Auto_InspectionDocs> InspectionDocs { get; set; } = new List<Auto_InspectionDocs>();
    }
}