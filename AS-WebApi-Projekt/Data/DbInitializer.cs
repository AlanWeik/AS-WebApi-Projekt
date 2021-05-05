using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AS_WebApi_Projekt.Data;
using AS_WebApi_Projekt.Models;

namespace AS_WebApi_Projekt.Data
{
    public class DbInitializer
    {
        public static void Initialize(AS_WebApi_ProjektContext context)
        {
            context.Database.EnsureCreated();

            context.RemoveRange(context.Users);
            context.RemoveRange(context.GeoMessage);

            var users = new User[]
            {
                new User { firstName="Alan", lastName ="Weik", Email="SomeEmail@gmail.com"},
                new User { firstName="Svante", lastName ="Pålsson", Email="SomeEmail@gmail.com"},
                new User { firstName="Tom", lastName ="Pålsson", Email="SomeEmail@gmail.com"},
                new User { firstName="Lana", lastName ="Pålsson", Email="SomeEmail@gmail.com"},
            };
            context.Users.AddRange(users);
            context.SaveChanges();

            var geoMessages = new GeoMessage[]
            {
                new GeoMessage { message = "Absinth dricks bäst av lösa människor.", longitude = -72.523,latitude = 593.232 },
                new GeoMessage { message = "Måttfullhet är ett ganska fult ord.", longitude = -104.45,latitude = 332.523 },
                new GeoMessage { message = "Det regnar, ta med ett paraply.", longitude = -332.523,latitude = 124.245 },
                new GeoMessage { message = "Fett mycket nudlar.", longitude = -124.245,latitude = 72.523 },
            };
            context.GeoMessage.AddRange(geoMessages);
            context.SaveChanges();

        }
    }
}   