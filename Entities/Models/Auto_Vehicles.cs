using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Helpers.Enumerations;

namespace Entities.Models
{
    public partial class Auto_Vehicles
    {
        [Key]
        public Guid IDPK_Vehicle { get; set; }

        [Required]
        public string IDFK_Owner { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string PlateNumber { get; set; } = null!;

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
        public VehicleStatus Status { get; set; } = VehicleStatus.Pending;

        [MaxLength(500)]
        public string? ApprovalComment { get; set; }

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

        // Relationships
        [ForeignKey("IDFK_Owner")]
        public virtual Auto_Users Owner { get; set; } = null!;

        [ForeignKey("CreatedBy")]
        public virtual Auto_Users CreatedByUser { get; set; } = null!;

        [ForeignKey("ModifiedBy")]
        public virtual Auto_Users? ModifiedByUser { get; set; }

        [InverseProperty(nameof(Auto_VehicleChangeRequests.Vehicle))]
        public virtual ICollection<Auto_VehicleChangeRequests> VehicleChangeRequests { get; set; } = new List<Auto_VehicleChangeRequests>();
    }
}
