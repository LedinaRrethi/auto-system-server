using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.InspectionDTO
{
    public class InspectionDocDTO
    {
        public Guid IDPK_InspectionDoc { get; set; }
        public string DocumentName { get; set; } = null!;
        public string FileBase64 { get; set; } = null!;
    }

}
