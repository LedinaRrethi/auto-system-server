using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.VehicleDTO
{
    public class VehicleRegisterDTO
    {
        public string PlateNumber { get; set; } = null!;

        public string Color { get; set; } = null!;

        [Required]
        public byte SeatCount { get; set; }

        [Required]
        public byte DoorCount { get; set; }

        public string ChassisNumber { get; set; } = null!;
    }

}
