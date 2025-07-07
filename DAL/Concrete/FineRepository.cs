using DAL.Contracts;
using DTO.FineDTO;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Concrete
{
    public class FineRepository : BaseRepository<Auto_Fines>, IFineRepository
    {
        private readonly AutoSystemDbContext _context;

        public FineRepository(AutoSystemDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<Auto_Vehicles?> GetVehicleByPlateAsync(string plate) =>
            _context.Auto_Vehicles.FirstOrDefaultAsync(v => v.PlateNumber == plate && v.Invalidated == 0);

        public Task<Auto_Users?> GetUserByIdAsync(string userId) =>
            _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        public Task AddFineRecipientAsync(Auto_FineRecipients recipient) =>
            _context.Auto_FineRecipients.AddAsync(recipient).AsTask();

        public Task AddFineAsync(Auto_Fines fine) =>
            _context.Auto_Fines.AddAsync(fine).AsTask();

        public Task<Auto_FineRecipients?> GetFineRecipientByUserIdAsync(string userId) =>
            _context.Auto_FineRecipients.FirstOrDefaultAsync(r => r.IDFK_User == userId && r.Invalidated == 0);

        public Task<Auto_FineRecipients?> GetFineRecipientByPersonalIdAsync(string personalId) =>
            _context.Auto_FineRecipients.FirstOrDefaultAsync(r => r.PersonalId == personalId && r.Invalidated == 0);

        public Task<Auto_FineRecipients?> GetFineRecipientByPlateAsync(string plate) =>
            _context.Auto_FineRecipients.FirstOrDefaultAsync(r => r.PlateNumber == plate && r.Invalidated == 0);

        public async Task<List<Auto_Fines>> GetFinesCreatedByPoliceAsync(string policeId)
        {
            return await _context.Auto_Fines
                .Include(f => f.FineRecipient)
                .Include(f => f.Vehicle)
                .Include(f => f.PoliceOfficer)
                .Where(f => f.CreatedBy == policeId && f.Invalidated == 0)
                .ToListAsync();
        }

        public async Task<List<Auto_Fines>> GetFinesForUserAsync(string userId)
        {
            return await _context.Auto_Fines
                .Include(f => f.FineRecipient)
                .Include(f => f.Vehicle)
                .Include(f => f.PoliceOfficer)
                .Where(f => f.FineRecipient.IDFK_User == userId && f.Invalidated == 0)
                .ToListAsync();
        }

        public async Task<List<Auto_Fines>> GetAllFinesAsync()
        {
            return await _context.Auto_Fines
                .Include(f => f.FineRecipient)
                .Include(f => f.Vehicle)
                .Include(f => f.PoliceOfficer)
                .Where(f => f.Invalidated == 0)
                .ToListAsync();
        }

        public async Task<int> CountFinesByPoliceAsync(string policeId)
        {
            return await _context.Auto_Fines
                .CountAsync(f => f.CreatedBy == policeId && f.Invalidated == 0);
        }

        public void UpdateFineRecipient(Auto_FineRecipients recipient)
        {
            _context.Auto_FineRecipients.Update(recipient);
        }


        public Task SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
