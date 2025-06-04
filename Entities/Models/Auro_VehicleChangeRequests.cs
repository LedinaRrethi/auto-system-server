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
    public class Auto_VehicleChangeRequests
    {
        [Key]
        public Guid IDPK_ChangeRequest { get; set; }

        [Required]
        public Guid IDFK_Vehicle { get; set; }

        [Required]
        public Guid IDFK_Requester { get; set; } 

        [Required]
        public byte RequestType { get; set; } // 0 = update, 1 = delete

        [Required]
        public string RequestDataJson { get; set; } = null!; // vlerat e reja në JSON

        [Required]
        public string CurrentDataSnapshotJson { get; set; } = null!; // vlerat ekzistuese në JSON

        public string? RequesterComment { get; set; } 

        public byte Status { get; set; } = 0; // 0 = pending, 1 = approved, 2 = rejected

        public string? AdminComment { get; set; } 


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
        [ForeignKey("IDFK_Requester")]
        public virtual Auto_Users Requester { get; set; } = null!;

        [ForeignKey("IDFK_Vehicle")]
        public virtual Auto_Vehicles Vehicle { get; set; } = null!;

        [ForeignKey("CreatedBy")]
        public virtual Auto_Users CreatedByUser { get; set; } = null!;

        [ForeignKey("ModifiedBy")]
        public virtual Auto_Users? ModifiedByUser { get; set; }
    }
}
