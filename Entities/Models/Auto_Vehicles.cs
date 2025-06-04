using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Auto_Vehicles
    {
        [Key]
        public Guid IDPK_Vehicle { get; set; }

        [Required]
        public Guid IDFK_Owner { get; set; }

        [Required]
        [MaxLength(20)]
        public string PlateNumber { get; set; } = null!; // unique constraint in DbContext

        [Required]
        [MaxLength(50)]
        public string Color { get; set; } = null!;

        [Required]
        public byte SeatCount { get; set; }

        [Required]
        public byte DoorCount { get; set; }

        [Required]
        [MaxLength(50)]
        public string ChassisNumber { get; set; } = null!;

        [Required]
        public byte Status { get; set; }  // 0 = pending, 1 = approved, 2 = rejected

        [MaxLength(500)]
        public string? ApprovalComment { get; set; }

        // Audit Fields
        public byte Invalidated { get; set; } = 0;

        [Required]
        public Guid CreatedBy { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [MaxLength(46)]
        public string CreatedIp { get; set; }

        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        [MaxLength(46)]
        public string? ModifiedIp { get; set; }

        // Relationships
        [ForeignKey("IDFK_Owner")]
        public virtual Auto_Users Owner { get; set; } = null!;

        [ForeignKey("CreatedBy")]
        public virtual Auto_Users CreatedByUser { get; set; } = null!;

        [ForeignKey("ModifiedBy")]
        public virtual Auto_Users? ModifiedByUser { get; set; }
    }
}
