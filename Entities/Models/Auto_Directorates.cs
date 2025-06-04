using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public partial class Auto_Directorates
    {
        [Key]
        public Guid IDPK_Directory { get; set; }

        [Required]
        [MaxLength(50)]
        public string DirectoryName { get; set; } = null!;

        [Required]
        public string Address { get; set; } = null!;

        public virtual ICollection<Auto_Users>? Specialists { get; set; } = new List<Auto_Users>();

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

        [ForeignKey("CreatedBy")]
        public virtual Auto_Users CreatedByUser { get; set; } = null!;

        [ForeignKey("ModifiedBy")]
        public virtual Auto_Users? ModifiedByUser { get; set; }
    }
}
