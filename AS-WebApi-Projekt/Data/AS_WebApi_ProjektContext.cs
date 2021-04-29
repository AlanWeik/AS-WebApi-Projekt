using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AS_WebApi_Projekt.Models;

namespace AS_WebApi_Projekt.Data
{
    public class AS_WebApi_ProjektContext : DbContext
    {
        public AS_WebApi_ProjektContext (DbContextOptions<AS_WebApi_ProjektContext> options)
            : base(options)
        {
        }

        public DbSet<AS_WebApi_Projekt.Models.GeoMessage> GeoMessage { get; set; }
        public DbSet<AS_WebApi_Projekt.Models.GeoMessage> Users { get; set; }


        public async Task SeedDb(AS_WebApi_ProjektContext context)
        {
            await Database.EnsureDeletedAsync();
            await Database.EnsureCreatedAsync();

            IList<User> Users = new List<User>();
            IList<GeoMessage> geoMessages = new List<GeoMessage>();


            Users.Add(new User()
            {
                firstName = "Svante",
                lastName = "Pålsson"
            });

            Users.Add(new User()
            {
                firstName = "Alan",
                lastName = "Weik"
            });

            geoMessages.Add(new GeoMessage()
            {
                message = "Absinth dricks bäst av lösa människor",
                longitude = -124.245,
                latitude = 332.523
            });

            geoMessages.Add(new GeoMessage()
            {
                message = "Måttfullhet är ett ganska fult ord",
                longitude = 593.232,
                latitude = 72.523
            });


            context.SaveChanges();
        }
    }
}
