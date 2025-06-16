using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.FineDTO
{
    public class FineRecipientDTO
    {
        public Guid IDPK_FineRecipient { get; set; }
        public string? IDFK_User { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? FatherName { get; set; }
        public string? PersonalId { get; set; }
        public string? PlateNumber { get; set; }
        public string? PhoneNumber { get; set; }

        public byte Invalidated { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public string? CreatedIp { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedIp { get; set; }
    }

}
