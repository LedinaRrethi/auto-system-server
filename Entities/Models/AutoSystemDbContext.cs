using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Entities.Models
{
    public class AutoSystemDbContext : IdentityDbContext<IdentityUser>
    {
        public AutoSystemDbContext(DbContextOptions<AutoSystemDbContext> options)
            : base(options) { }

        // Add custom DbSets later if needed
    }
}
