using DAL.Contracts;
using DTO.FineDTO;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DAL.Concrete
{
    public class FineRepository : IFineRepository
    {
        private readonly AutoSystemDbContext _context;

        public FineRepository(AutoSystemDbContext context) => _context = context;


        //Gjen automjetin me targën përkatëse që nuk është fshirë logjikisht(Invalidated == 0)
        public Task<Auto_Vehicles?> GetVehicleByPlateAsync(string plate) =>
            _context.Auto_Vehicles.FirstOrDefaultAsync(v => v.PlateNumber == plate && v.Invalidated == 0);


        // Merr përdoruesin nëse ekziston në databazë (përdoret për të mbushur të dhënat në FineRecipient).
        public Task<Auto_Users?> GetUserByIdAsync(string userId) =>
            _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        public Task AddFineRecipientAsync(Auto_FineRecipients recipient) =>
            _context.Auto_FineRecipients.AddAsync(recipient).AsTask();

        public Task AddFineAsync(Auto_Fines fine) =>
            _context.Auto_Fines.AddAsync(fine).AsTask();

        public Task SaveChangesAsync() => _context.SaveChangesAsync();


        public async Task<List<Auto_Fines>> GetFinesForUserAsync(string userId, FineFilterDTO filter, int page, int pageSize)
        {
            var query = _context.Auto_Fines
                .Include(f => f.FineRecipient)
                .Include(f => f.PoliceOfficer)
                .Where(f => f.FineRecipient.IDFK_User == userId && f.Invalidated == 0);

            if (filter.FromDate.HasValue)
                query = query.Where(f => f.FineDate >= filter.FromDate.Value);
            if (filter.ToDate.HasValue)
                query = query.Where(f => f.FineDate <= filter.ToDate.Value);

            return await query.OrderByDescending(f => f.FineDate)
                              .Skip((page - 1) * pageSize)
                              .Take(pageSize).ToListAsync();
        }

        public Task<List<Auto_Fines>> SearchFinesByPlateAsync(string plate, int page, int pageSize) =>
            _context.Auto_Fines.Include(f => f.FineRecipient)
                .Include(f => f.PoliceOfficer)
                .Where(f => f.FineRecipient.PlateNumber != null && f.FineRecipient.PlateNumber.Contains(plate))
                .OrderByDescending(f => f.FineDate)
                .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();


        public async Task<List<Auto_Fines>> GetAllFinesAsync(int page, int pageSize)
        {
            return await _context.Auto_Fines
                .Include(f => f.FineRecipient)
                .Include(f => f.Vehicle)
                .Include(f => f.PoliceOfficer)
                .Where(f => f.Invalidated == 0)
                .OrderByDescending(f => f.FineDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }


    }

}
