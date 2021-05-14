using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AS_WebApi_Projekt.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using AS_WebApi_Projekt.APIKey;
using Microsoft.Extensions.DependencyInjection;

namespace AS_WebApi_Projekt.Data
{
    public class  AS_WebApi_ProjektContext : IdentityDbContext<User>
    {
        public DbSet<AS_WebApi_Projekt.Models.GeoMessageV2> GeoMessageV2 { get; set; }
        //public DbSet<AS_WebApi_Projekt.Models.GeoMessageV1> GeoMessageV1 { get; set; }
        public DbSet<AS_WebApi_Projekt.Models.Message> Message { get; set; }
        public DbSet<AS_WebApi_Projekt.Models.User> User { get; set; }
        public DbSet<AS_WebApi_Projekt.Models.ApiToken> ApiTokens { get; set; }
    
        public AS_WebApi_ProjektContext(DbContextOptions<AS_WebApi_ProjektContext> options)
            : base(options)
        {
        }
        // * Svantes AUTH.
        // * Identity. 



        public static async Task Reset(IServiceProvider provider)
        {
            var context = provider.GetRequiredService<AS_WebApi_ProjektContext>();
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            var userManager = provider.GetRequiredService<UserManager<IdentityUser>>();

            await userManager.CreateAsync(
                new IdentityUser
                {
                    UserName = "TestUser"
                },
                "QWEqwe123!\"#");
        }
    }
}



