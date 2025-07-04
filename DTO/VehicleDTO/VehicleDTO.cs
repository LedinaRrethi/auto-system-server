using Helpers.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.VehicleDTO
{
    /// <DTO i plote >
    public class VehicleDTO
    {
        public Guid IDPK_Vehicle { get; set; }
        public string PlateNumber { get; set; } = null!;
        public string Color { get; set; } = null!;
        public byte SeatCount { get; set; }
        public byte DoorCount { get; set; }
        public string ChassisNumber { get; set; } = null!;
        public VehicleStatus Status { get; set; }
        public string? ApprovalComment { get; set; }
        public DateTime CreatedOn { get; set; }
    }


}
