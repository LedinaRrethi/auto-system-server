using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.InspectionDTO
{
    public class InspectionDTO
    {
        public Guid IDPK_Inspection { get; set; }
        public Guid IDFK_InspectionRequest { get; set; }
        public bool IsPassed { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedOn { get; set; }
    }

}
