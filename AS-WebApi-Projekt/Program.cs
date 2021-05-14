using AS_WebApi_Projekt.Data;
using AS_WebApi_Projekt.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AS_WebApi_Projekt
{
    public class Program
    {
        public class MainProgram
        {
            public static void Main(string[] args)
            {
                var host = CreateHostBuilder(args).Build();

                using (var scope = host.Services.CreateScope())
                {
                    var provider = scope.ServiceProvider;

                    Data.AS_WebApi_ProjektContext.Reset(provider).Wait();
                }

                host.Run();
            }

            public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }

}
