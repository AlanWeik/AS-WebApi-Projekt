using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AS_WebApi_Projekt.Models;
using AS_WebApi_Projekt.Models.v2;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using AS_WebApi_Projekt.APIKey;

namespace AS_WebApi_Projekt.Data
{
    public class AS_WebApi_ProjektContext : IdentityDbContext<User>
    {
        public AS_WebApi_ProjektContext(DbContextOptions<AS_WebApi_ProjektContext> options)
            : base(options)
        {
        }
        // * Svantes AUTH.
        // * Identity. 
        public DbSet<AS_WebApi_Projekt.Models.v2.GeoMessageV2> GeoMessageV2 { get; set; }
        public DbSet<AS_WebApi_Projekt.Models.GeoMessageV1> GeoMessageV1 { get; set; }
        public DbSet<AS_WebApi_Projekt.Models.v2.Message> Message { get; set; }
        public DbSet<AS_WebApi_Projekt.Models.User> User { get; set; }
        public DbSet<AS_WebApi_Projekt.Models.ApiToken> ApiTokens { get; set; }


        public async Task SeedDb(UserManager<User> userManager)
        {
            //context.Database.EnsureCreated();
            //Database.EnsureDeletedAsync();
            //Database.EnsureCreatedAsync();

            await Database.MigrateAsync();

            User admin1 = new User()
            { firstName = "Alan", lastName = "Weik", UserName = "admin1", Email = "mail@mail.com", EmailConfirmed = true };
            User admin2 = new User()
            { firstName = "Svante", lastName = "Pålsson", UserName = "admin2", Email = "mail@mail.com", EmailConfirmed = true };

            await userManager.CreateAsync(admin1, "Test123!");
            await userManager.CreateAsync(admin2, "Test123!");

            TokenManager getToken = new TokenManager(this);
            await getToken.GenerateTokenAsync(admin1);
            await getToken.GenerateTokenAsync(admin2);
            await SaveChangesAsync();
        }
    }
}


