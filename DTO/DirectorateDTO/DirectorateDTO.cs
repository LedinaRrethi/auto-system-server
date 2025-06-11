using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DirectorateDTO
{
    public class DirectorateDTO
    {
        public Guid Id { get; set; }

        [Required, MaxLength(50)]
        public string DirectoryName { get; set; } = null!;
    }

}
