using DTO.FineDTO;
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

        // Shton një gjobë të re në databazë dhe e lidh me FineRecipient.
        Task AddFineAsync(Auto_Fines fine);
        Task SaveChangesAsync();


        // Merr listën e gjobave për një përdorues të caktuar (Individ), me mundësi filtrimi dhe pagination.
        Task<List<Auto_Fines>> GetFinesForUserAsync(string userId, FineFilterDTO filter, int page, int pageSize);

        // Kërkon gjobat sipas targës së automjetit për Policin, me pagination.
        Task<List<Auto_Fines>> SearchFinesByPlateAsync(string plate, int page, int pageSize);

        Task<List<Auto_Fines>> GetAllFinesAsync(int page, int pageSize);


    }

}
