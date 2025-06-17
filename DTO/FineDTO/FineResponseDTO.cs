using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.FineDTO
{
    public class FineResponseDTO
    {
        public Guid IDPK_Fine { get; set; }
        public decimal FineAmount { get; set; }
        public string? FineReason { get; set; }
        public DateTime FineDate { get; set; }

        public string PoliceFullName { get; set; } = null!;

        public string? RecipientFullName { get; set; }

        public string? PlateNumber { get; set; }
    }

}
