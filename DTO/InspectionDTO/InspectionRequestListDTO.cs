using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.InspectionDTO
{
    public class InspectionRequestListDTO
    {
        public Guid IDPK_InspectionRequest { get; set; } // kolone ne Auto_InspectionRequest

        public Guid IDPK_Inspection { get; set; }
        public string PlateNumber { get; set; } = string.Empty; // kolone ne Auto_Vehicles
        public DateTime RequestedDate { get; set; } // kolone ne Auto_InspectionRequest
        public string InspectionDate => RequestedDate.ToString("dd/MM/yyyy"); 
        public string InspectionTime => RequestedDate.ToString("HH:mm");
        public string Status { get; set; } = string.Empty; //kolone ne Auto_InspectionRequest
        public string? Comment { get; set; } // kolone ne Auto_Inspection
        public bool? IsPassed { get; set; } // kolone ne Auto_Inspection
        public List<InspectionDocumentDTO> Documents { get; set; } = new(); 
    }
}
