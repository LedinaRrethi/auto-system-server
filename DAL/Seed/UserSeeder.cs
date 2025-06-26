using Entities.Models;
using Helpers.Enumerations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DAL.Seed
{
    public static class UserSeeder
    {
        public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<Auto_Users>>();

            string adminEmail = "ledina@gmail.com";
            string password = "Ledina1!";

            var existingUser = await userManager.FindByEmailAsync(adminEmail);
            if (existingUser == null)
            {
                var admin = new Auto_Users
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FirstName = "Ledina",
                    FatherName = "Ferdinant",
                    LastName = "Rrethi",
                    BirthDate = new DateTime(2003, 2, 28),
                    IsSpecialist = false,
                    Status = UserStatus.Approved,
                    Invalidated = 0,
                    CreatedBy = "Seeder",
                    CreatedOn = DateTime.UtcNow,
                    CreatedIp = "127.0.0.1",
                };

                var result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                    Console.WriteLine("Admin user created.");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error creating admin: {error.Description}");
                    }
                }
            }
        }
    }
}
