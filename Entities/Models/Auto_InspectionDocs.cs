using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public partial class Auto_InspectionDocs
    {
        [Key]
        public Guid IDPK_InspectionDoc { get; set; }

        [Required]
        public Guid IDFK_Inspection { get; set; }

        [Required]
        [MaxLength(255)]
        public string DocumentName { get; set; } = null!;

        [Required]
        [MaxLength(7_000_000)]
        public string FileBase64 { get; set; } = null!;

        public byte Invalidated { get; set; } = 0;

        [Required]
        public required string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [MaxLength(46)]
        public string? CreatedIp { get; set; }

        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        [MaxLength(46)]
        public string? ModifiedIp { get; set; }

        [ForeignKey(nameof(IDFK_Inspection))]
        public virtual Auto_Inspections Inspection { get; set; } = null!;

        [ForeignKey(nameof(CreatedBy))]
        public virtual Auto_Users CreatedByUser { get; set; } = null!;

        [ForeignKey(nameof(ModifiedBy))]
        public virtual Auto_Users? ModifiedByUser { get; set; }
    }
}
