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
    }
}
