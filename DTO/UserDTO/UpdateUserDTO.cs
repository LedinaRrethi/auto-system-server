using Helpers.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.UserDTO
{
    public class UpdateUserDTO
    {
        public string UserId { get; set; } = null!;

        public UserStatus Status { get; set; } 

        public string? ModifiedBy { get; set; }
        public string? ModifiedIp { get; set; }
    }
}
