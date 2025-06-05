using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public partial class Auto_Users : IdentityUser
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string FatherName { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = null!;

        [Required]
        public bool IsSpecialist { get; set; } = false;

        [MaxLength(50)]
        public string? SpecialistNumber { get; set; }

        public Guid? IDFK_Directory { get; set; }

        public bool IsApproved { get; set; } = false;

        // Audit Fields
        public Byte Invalidated { get; set; } = 0;

        [Column("CreatedBy")]
        public string? CreatedBy { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [MaxLength(46)]
        public string? CreatedIp { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [MaxLength(46)]
        public string? ModifiedIp { get; set; }

        // Relationships 
        [ForeignKey("IDFK_Directory")]
        public virtual Auto_Directorates? Directorate { get; set; }
    }
}
