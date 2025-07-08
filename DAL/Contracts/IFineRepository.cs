using DTO.FineDTO;
using DTO.VehicleDTO;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contracts
{
    public interface IFineRepository
    {
        //polici vendos gjobe , dhe duhet te dije qe automjeti eshte regjistruar
        Task<Auto_Vehicles?> GetVehicleByPlateAsync(string plate);

        //merr pronarin e automjetit , nese ekziston ne sistem is user i loguar 
        Task<Auto_Users?> GetUserByIdAsync(string userId);

        //shton nje gjobe ne db
        //krijon nje rekord te pronarit te automjetit pavaresisht nese eshte apo jo i regjistruar
        Task AddFineRecipientAsync(Auto_FineRecipients recipient);
        Task AddFineAsync(Auto_Fines fine);

        Task<Auto_FineRecipients?> GetFineRecipientByUserIdAsync(string userId);
        Task<Auto_FineRecipients?> GetFineRecipientByPersonalIdAsync(string personalId);

        Task<Auto_FineRecipients?> GetFineRecipientByPlateAsync(string plate);

        Task<List<Auto_Fines>> GetFinesCreatedByPoliceAsync(string policeId);
        Task<List<Auto_Fines>> GetFinesForUserAsync(string userId);
        
        IQueryable<Auto_Fines> QueryAllFines();

        Task<int> CountFinesByPoliceAsync(string policeId);

        void UpdateFineRecipient(Auto_FineRecipients recipient);

        Task<List<Auto_Fines>> GetFinesByPlateWithoutVehicleAsync(string plateNumber);

        Task UpdateAsync(Auto_Fines fine);

        Task<int> CountFinesForUserAsync(string userId);

        Task SaveChangesAsync();











    }

}
