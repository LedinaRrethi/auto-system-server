using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Entities.Models
{
    public class AutoUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SpecialistNumber {  get; set; }
        public string IDFK_Directorate { get; set; }

        //Audit Fields
        public bool Invalidated { get; set; } = false;
        public long? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedIp { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedIp { get; set; }
    }
}
