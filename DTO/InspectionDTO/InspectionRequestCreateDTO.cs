using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.InspectionDTO
{
    public class InspectionRequestCreateDTO
    {
        public Guid IDFK_Vehicle { get; set; }
        public Guid IDFK_Directory { get; set; }
        public DateTime RequestedDate { get; set; }
    }

}
