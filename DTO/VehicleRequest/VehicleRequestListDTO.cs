using Helpers.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.VehicleRequest
{
    public class VehicleRequestListDTO
    {
        public Guid IDPK_ChangeRequest { get; set; }

        public Guid IDFK_Vehicle { get; set; }

        public string? PlateNumber { get; set; }

        public ChangeRequestType RequestType { get; set; }

        public ChangeRequestStatus Status { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
