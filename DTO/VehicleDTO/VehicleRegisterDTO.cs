using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.VehicleDTO
{
    /// <Perdoret nga useri per te regjistruar nje automjet>
    public class VehicleRegisterDTO
    {
        [Required]
        [MaxLength(20)]
        public string PlateNumber { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Color { get; set; } = null!;

        [Required]
        public byte SeatCount { get; set; }

        [Required]
        public byte DoorCount { get; set; }

        [Required]
        [MaxLength(50)]
        public string ChassisNumber { get; set; } = null!;
    }

}
