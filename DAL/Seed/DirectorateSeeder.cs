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
                var creatorId = "070d74c3-7550-4b5a-a618-42bc327fdd0b";
                var createdOn = DateTime.UtcNow;
                var createdIp = "127.0.0.1";

                var defaultDirectorates = new List<Auto_Directorates>
                {
                    new Auto_Directorates { IDPK_Directory = Guid.NewGuid(), DirectoryName = "Tirana", Address = "Rruga e Dibrës", CreatedBy = creatorId, CreatedOn = createdOn, CreatedIp = createdIp },
                    new Auto_Directorates { IDPK_Directory = Guid.NewGuid(), DirectoryName = "Durrës", Address = "Sheshi Liria", CreatedBy = creatorId, CreatedOn = createdOn, CreatedIp = createdIp },
                    new Auto_Directorates { IDPK_Directory = Guid.NewGuid(), DirectoryName = "Shkodra", Address = "Lagjja Parrucë", CreatedBy = creatorId, CreatedOn = createdOn, CreatedIp = createdIp },
                    new Auto_Directorates { IDPK_Directory = Guid.NewGuid(), DirectoryName = "Elbasan", Address = "Rruga Qemal Stafa", CreatedBy = creatorId, CreatedOn = createdOn, CreatedIp = createdIp },
                    new Auto_Directorates { IDPK_Directory = Guid.NewGuid(), DirectoryName = "Fier", Address = "Rruga Jakov Xoxa", CreatedBy = creatorId, CreatedOn = createdOn, CreatedIp = createdIp },
                    new Auto_Directorates { IDPK_Directory = Guid.NewGuid(), DirectoryName = "Vlorë", Address = "Rruga Kosova", CreatedBy = creatorId, CreatedOn = createdOn, CreatedIp = createdIp },
                    new Auto_Directorates { IDPK_Directory = Guid.NewGuid(), DirectoryName = "Korçë", Address = "Rruga Republika", CreatedBy = creatorId, CreatedOn = createdOn, CreatedIp = createdIp },
                    new Auto_Directorates { IDPK_Directory = Guid.NewGuid(), DirectoryName = "Gjirokastër", Address = "Rruga e Zejtareve", CreatedBy = creatorId, CreatedOn = createdOn, CreatedIp = createdIp },
                    new Auto_Directorates { IDPK_Directory = Guid.NewGuid(), DirectoryName = "Berat", Address = "Rruga Antipatrea", CreatedBy = creatorId, CreatedOn = createdOn, CreatedIp = createdIp },
                    new Auto_Directorates { IDPK_Directory = Guid.NewGuid(), DirectoryName = "Dibër", Address = "Rruga e Çerçiz Topulli, Peshkopi", CreatedBy = creatorId, CreatedOn = createdOn, CreatedIp = createdIp },
                    new Auto_Directorates { IDPK_Directory = Guid.NewGuid(), DirectoryName = "Kukës", Address = "Rruga Fan S. Noli", CreatedBy = creatorId, CreatedOn = createdOn, CreatedIp = createdIp },
                    new Auto_Directorates { IDPK_Directory = Guid.NewGuid(), DirectoryName = "Lezhë", Address = "Rruga Besëlidhja", CreatedBy = creatorId, CreatedOn = createdOn, CreatedIp = createdIp }
                };

                await context.Auto_Directorates.AddRangeAsync(defaultDirectorates);
                await context.SaveChangesAsync();
            }
        }
    }
}
