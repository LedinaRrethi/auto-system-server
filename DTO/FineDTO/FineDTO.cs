using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.FineDTO
{
    public class FineDTO
    {
        public Guid IDPK_Fine { get; set; }
        public Guid? IDFK_Vehicle { get; set; }
        public Guid IDFK_FineRecipient { get; set; }
        public decimal FineAmount { get; set; }
        public DateTime FineDate { get; set; }
        public string? FineReason { get; set; }

        public byte Invalidated { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public string? CreatedIp { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedIp { get; set; }
    }

}
