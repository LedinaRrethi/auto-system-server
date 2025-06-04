using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public partial class Auto_Inspections
    {
        [Key]
        public Guid IDPK_Inspection { get; set; }
        public Guid IDFK_InspectionRequest { get; set; }
        public string IDFK_Specialist { get; set; }
        public bool IsPassed { get; set; } = false;
        public string? Comment { get; set; }

        //Audit
        public Byte Invalidated { get; set; }

        [Required]
        public required string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        [MaxLength(46)]
        public string? CreatedIp { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        [MaxLength(46)]
        public string? ModifiedIp { get; set; }

        // Relationships
        [ForeignKey("IDFK_InspectionRequest")]
        public virtual Auto_InspectionRequests Request { get; set; } = null!;

        [ForeignKey("IDFK_Specialist")]
        public virtual Auto_Users Specialist { get; set; } = null!;

        [ForeignKey("CreatedBy")]
        public virtual Auto_Users CreatedByUser { get; set; } = null!;

        [ForeignKey("ModifiedBy")]
        public virtual Auto_Users? ModifiedByUser { get; set; }
    }
}
