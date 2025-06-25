using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.InspectionDTO
{
    public class InspectionApprovalDTO
    {
        public Guid IDPK_Inspection { get; set; }
        public bool IsPassed { get; set; }     
        public string Comment { get; set; } = string.Empty;
        public List<InspectionDocumentDTO> Documents { get; set; } = new();
    }
}
