using Helpers.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.VehicleRequest
{
    /// <individi kerkon update apo delete>
    public class VehicleRequestDTO
    {
        [Required]
        public Guid VehicleId { get; set; }

        [Required]
        public ChangeRequestType RequestType { get; set; }

        [Required]
        public string RequestDataJson { get; set; } = null!;

        [Required]
        public string CurrentDataSnapshotJson { get; set; } = null!;

        public string? RequesterComment { get; set; }
    }
}
