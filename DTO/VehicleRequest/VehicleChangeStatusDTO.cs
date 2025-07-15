using Helpers.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace DTO.VehicleRequest
{
    /// <Perdoret nga admini per te ndryshuar statusin e kerkeses>
    public class VehicleChangeStatusDTO
    {
        [Required]
        public VehicleStatus NewStatus { get; set; } 

        [MaxLength(500)]
        public string? ApprovalComment { get; set; }
    }

}
