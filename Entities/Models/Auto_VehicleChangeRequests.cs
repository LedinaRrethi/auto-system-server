using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Helpers.Enumerations;

namespace Entities.Models
{
    public partial class Auto_VehicleChangeRequests
    {
        [Key]
        public Guid IDPK_ChangeRequest { get; set; }

        [Required]
        public Guid IDFK_Vehicle { get; set; }

        [Required]
        public string IDFK_Requester { get; set; }

        [Required]
        public ChangeRequestType RequestType { get; set; } 

        [Required]
        public string RequestDataJson { get; set; } = null!; // vlerat e reja në JSON

        [Required]
        public string CurrentDataSnapshotJson { get; set; } = null!; // vlerat ekzistuese në JSON

        public string? RequesterComment { get; set; } 

        public ChangeRequestStatus Status { get; set; } = 0; 

        public string? AdminComment { get; set; } 


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
