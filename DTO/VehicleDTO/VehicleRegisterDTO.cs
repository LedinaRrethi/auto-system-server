using System;
using System.ComponentModel.DataAnnotations;

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
