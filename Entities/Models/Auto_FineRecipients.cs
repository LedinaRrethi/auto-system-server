using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public partial class Auto_FineRecipients
    {
        [Key]
        public Guid IDPK_FineRecipient { get; set; }

        public string? IDFK_User { get; set; } 

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = null!;

        [MaxLength(50)]
        public string? FatherName { get; set; }

        [MaxLength(20)]
        public string? PersonalId { get; set; }

        [MaxLength(20)]
        public string? PlateNumber { get; set; }

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }


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
        [ForeignKey("IDFK_User")]
        public virtual Auto_Users? User { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual Auto_Users CreatedByUser { get; set; } = null!;


        [ForeignKey("ModifiedBy")]
        public virtual Auto_Users? ModifiedByUser { get; set; }
    }
}
