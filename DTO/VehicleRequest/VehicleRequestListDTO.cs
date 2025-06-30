using Helpers.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.VehicleRequest
{
    public class VehicleRequestListDTO
    {
        public Guid IDPK_ChangeRequest { get; set; }

        public Guid IDFK_Vehicle { get; set; }
        public ChangeRequestType RequestType { get; set; }

        public string RequestDataJson { get; set; } = null!;

        public string CurrentDataSnapshotJson { get; set; } = null!;

        public string? PlateNumber { get; set; }

        public VehicleStatus Status { get; set; } 

        public DateTime CreatedOn { get; set; }
    }
}
