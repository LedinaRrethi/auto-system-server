using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.InspectionDTO
{
    public class MyInspectionRequestDTO
    {
        public Guid IDPK_InspectionRequest { get; set; }
        public string PlateNumber { get; set; } = null!;
        public DateTime RequestedDate { get; set; }
        public string DirectorateName { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string? Comment { get; set; }
        public List<InspectionDocumentDTO> Documents { get; set; } = new();
    } 

}
