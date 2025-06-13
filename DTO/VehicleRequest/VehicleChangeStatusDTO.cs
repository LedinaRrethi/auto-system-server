using Helpers.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.VehicleRequest
{
    /// <Perdoret nga admini per te ndryshuar statusin e kerkeses>
    public class VehicleChangeStatusDTO
    {
        [Required]
        public ChangeRequestStatus NewStatus { get; set; } 

        [MaxLength(500)]
        public string? AdminComment { get; set; }
    }

}
