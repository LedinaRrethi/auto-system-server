using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.InspectionDTO
{
    public class InspectionDocumentUploadDTO
    {
        public Guid IDFK_InspectionRequest { get; set; }
        public string DocumentName { get; set; } = null!;
        public string FileBase64 { get; set; } = null!;
    }
}

