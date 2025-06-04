using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Auto_Users : IdentityUser
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string? FatherName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(50)]
        public string? SpecialistNumber { get; set; }

        public Guid? IDFK_Directory { get; set; }

        public bool IsApproved { get; set; } = false;

        // Audit Fields
        public Byte Invalidated { get; set; } = 0;

        public Guid? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [MaxLength(46)]
        public string? CreatedIp { get; set; }

        public Guid? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [MaxLength(46)]
        public string? ModifiedIp { get; set; }

        // Relationships 
        [ForeignKey("IDFK_Directory")]
        public virtual Auto_Directorates? Directorate { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual Auto_Users? Creator { get; set; }

        [ForeignKey("ModifiedBy")]
        public virtual Auto_Users? Modifier { get; set; }
    }
}
