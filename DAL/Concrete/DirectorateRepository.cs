using DAL.Contracts;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DAL.Concrete
{
    public class DirectorateRepository : IDirectorateRepository
    {
        private readonly AutoSystemDbContext _context;
        public DirectorateRepository(AutoSystemDbContext context)
        {
            _context = context;
        }
        public async Task<List<Auto_Directorates>> GetAllActiveAsync()
        {
            return await _context.Auto_Directorates
                .Where(d => d.Invalidated==0)
                .ToListAsync();
        }

    }
}
