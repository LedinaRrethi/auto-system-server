using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.AdminnDTO
{
   
        public class AdminDTO
        {
            public string Id { get; set; } = null!;
            public string Name { get; set; } = null!;
            public string Role { get; set; } = null!;
            public string Email { get; set; } = null!;
            public string Status { get; set; } = null!;
            public DateTime CreatedAt { get; set; }
        }
}

