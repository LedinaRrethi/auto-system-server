using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Seed
{
    public static class DirectorateSeeder
    {
        public static async Task SeedAsync(AutoSystemDbContext context)
        {
            if (!await context.Auto_Directorates.AnyAsync())
            {
                var defaultDirectorates = new List<Auto_Directorates>
                {
                    new Auto_Directorates
                    {
                        IDPK_Directory = Guid.NewGuid(),
                        DirectoryName = "Tirana",
                        Address = "Rruga e Dibrës",
                        CreatedBy = "070d74c3-7550-4b5a-a618-42bc327fdd0b", 
                        CreatedOn = DateTime.UtcNow,
                        CreatedIp = "127.0.0.1"
                    },
                    new Auto_Directorates
                    {
                        IDPK_Directory = Guid.NewGuid(),
                        DirectoryName = "Durrës",
                        Address = "Sheshi Liria",
                        CreatedBy = "070d74c3-7550-4b5a-a618-42bc327fdd0b", 
                        CreatedOn = DateTime.UtcNow,
                        CreatedIp = "127.0.0.1"
                    }
                };

                await context.Auto_Directorates.AddRangeAsync(defaultDirectorates);
                await context.SaveChangesAsync();
            }
        }
    }
}
