using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Helpers.Enumerations;

namespace Entities.Models
{
    public partial class Auto_InspectionRequests
    {
        [Key]
        public Guid IDPK_InspectionRequest { get; set; }

        [Required]
        public Guid IDFK_Vehicle { get; set; }

        [Required]
        public Guid IDFK_Directory { get; set; }

        [Required]
        public DateTime RequestedDate { get; set; } = DateTime.UtcNow;

        public InspectionStatus Status { get; set; } = InspectionStatus.Pending;



        // Audit
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


        // Relationships

        [ForeignKey("IDFK_Vehicle")]
        public virtual Auto_Vehicles Vehicle { get; set; } = null!;

        [ForeignKey("IDFK_Directory")]
        public virtual Auto_Directorates Directory { get; set; } = null!;

        [ForeignKey("CreatedBy")]
        public virtual Auto_Users Requester { get; set; } = null!;

        [ForeignKey("ModifiedBy")]
        public virtual Auto_Users? ModifiedByUser { get; set; }
    }

}
