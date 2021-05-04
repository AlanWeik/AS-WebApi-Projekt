using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AS_WebApi_Projekt.Data;

namespace AS_WebApi_Projekt
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApiVersioning(o =>
            {
                o.DefaultApiVersion = new ApiVersion(2, 0);
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(o =>
            {
                o.SubstituteApiVersionInUrl = true;
                o.GroupNameFormat = "'v' VVV";
            });

            services.AddControllers();

            services.AddSwaggerGen(o =>
            {
                o.SwaggerDoc("v1.0", new OpenApiInfo 
                { 
                    Title = "AS_WebApi_Projekt", 
                    Version = "v1.0" 
                });

                o.SwaggerDoc("v2.0", new OpenApiInfo
                {
                    Title = "AS_WebApi_Projekt",
                    Version = "v2.0"
                });
            });

            services.AddDbContext<AS_WebApi_ProjektContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("AS_WebApi_ProjektContext")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
            }
            app.UseSwaggerUI(o =>
            {
                o.SwaggerEndpoint($"/swagger/v1.0/swagger.json", "AS_WebApi_Projekt v1.0");
                o.SwaggerEndpoint($"/swagger/v2.0/swagger.json", "AS_WebApi_Projekt v2.0");
            });
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
