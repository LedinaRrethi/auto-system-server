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
                var creatorId = "082ada57-0fa0-4542-8528-abacaa5e2858";
                var createdOn = DateTime.UtcNow;
                var createdIp = "127.0.0.1";

                var defaultDirectorates = new List<Auto_Directorates>
                {
                    new() { DirectoryName = "DPSHTRR Tirana", Address = "Rruga e Dibrës", CreatedBy = creatorId, CreatedOn = createdOn, CreatedIp = createdIp },
                    new() { DirectoryName = "DPSHTRR Durrës", Address = "Sheshi Liria", CreatedBy = creatorId, CreatedOn = createdOn, CreatedIp = createdIp },
                    new() { DirectoryName = "DPSHTRR Shkodër", Address = "Lagjja Parrucë", CreatedBy = creatorId, CreatedOn = createdOn, CreatedIp = createdIp },
                    new() { DirectoryName = "DPSHTRR Elbasan", Address = "Rruga Qemal Stafa", CreatedBy = creatorId, CreatedOn = createdOn, CreatedIp = createdIp },
                    new() { DirectoryName = "DPSHTRR Fier", Address = "Rruga Jakov Xoxa", CreatedBy = creatorId, CreatedOn = createdOn, CreatedIp = createdIp },
                    new() { DirectoryName = "DPSHTRR Vlorë", Address = "Rruga Kosova", CreatedBy = creatorId, CreatedOn = createdOn, CreatedIp = createdIp },
                    new() { DirectoryName = "DPSHTRR Korçë", Address = "Rruga Republika", CreatedBy = creatorId, CreatedOn = createdOn, CreatedIp = createdIp },
                    new() { DirectoryName = "DPSHTRR Gjirokastër", Address = "Rruga e Zejtareve", CreatedBy = creatorId, CreatedOn = createdOn, CreatedIp = createdIp },
                    new() { DirectoryName = "DPSHTRR Berat", Address = "Rruga Antipatrea", CreatedBy = creatorId, CreatedOn = createdOn, CreatedIp = createdIp },
                    new() { DirectoryName = "DPSHTRR Dibër", Address = "Rruga e Çerçiz Topulli, Peshkopi", CreatedBy = creatorId, CreatedOn = createdOn, CreatedIp = createdIp },
                    new() { DirectoryName = "DPSHTRR Kukës", Address = "Rruga Fan S. Noli", CreatedBy = creatorId, CreatedOn = createdOn, CreatedIp = createdIp },
                    new() { DirectoryName = "DPSHTRR Lezhë", Address = "Rruga Besëlidhja", CreatedBy = creatorId, CreatedOn = createdOn, CreatedIp = createdIp }
                };

                foreach (var d in defaultDirectorates)
                    d.IDPK_Directory = Guid.NewGuid();

                await context.Auto_Directorates.AddRangeAsync(defaultDirectorates);
                await context.SaveChangesAsync();
            }
        }
    }
}
