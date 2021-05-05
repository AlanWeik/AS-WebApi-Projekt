using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AS_WebApi_Projekt.Data;
using AS_WebApi_Projekt.Models;
using AS_WebApi_Projekt.Models.v2;

namespace AS_WebApi_Projekt.Data
{
    public class DbInitializer
    {
        public static void Initialize(AS_WebApi_ProjektContext context)
        {
            context.Database.EnsureCreated();

            context.RemoveRange(context.Users);
            context.RemoveRange(context.GeoMessageV2);


            var users = new User[]
            {
                new User { firstName="Alan", lastName ="Weik", Email="SomeEmail@gmail.com"},
                new User { firstName="Svante", lastName ="Pålsson", Email="SomeEmail@gmail.com"},
                new User { firstName="Alfons", lastName ="Åberg", Email="SomeEmail@gmail.com"},
                new User { firstName="James", lastName ="Brown", Email="SomeEmail@gmail.com"},
            };
            context.Users.AddRange(users);
            context.SaveChanges();

            var geoMessagesV2 = new GeoMessageV2[]
            {
                new GeoMessageV2 { message = "Absinth dricks bäst av lösa människor.", longitude = -72.523,latitude = 593.232 },
                new GeoMessageV2 { message = "Måttfullhet är ett ganska fult ord.", longitude = -104.45,latitude = 332.523 },
                new GeoMessageV2 { message = "Det regnar, ta med ett paraply.", longitude = -332.523,latitude = 124.245 },
                new GeoMessageV2 { message = "Fett mycket nudlar.", longitude = -124.245,latitude = 72.523 },
            };
            context.GeoMessageV2.AddRange(geoMessagesV2);
            context.SaveChanges();
        }
    }
}   