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
using AS_WebApi_Projekt.Models;
using Microsoft.AspNetCore.Authentication;
using AS_WebApi_Projekt.APIKey;
using Microsoft.AspNetCore.Identity;
using System.IO;

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
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.ReportApiVersions = true;
                o.UseApiBehavior = false;
            });

            services.AddVersionedApiExplorer(o =>
            {
                o.SubstituteApiVersionInUrl = true;
                o.GroupNameFormat = "'v'VVV";
            });
            services.AddControllers();
            services.AddSwaggerGen(o =>
            {
                o.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ApiDocu.xml"),true);
                o.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "AS_WebApi_Projekt", 
                    Version = "v1" 
                });

                o.SwaggerDoc("v2", new OpenApiInfo
                {
                    Title = "AS_WebApi_Projekt",
                    Version = "v2"
                });
            });

            services.AddDbContext<AS_WebApi_ProjektContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("AS_WebApi_ProjektContext")));
            services.AddDefaultIdentity<IdentityUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<AS_WebApi_ProjektContext>();
            services.AddAuthentication("MyAuthScheme")
                .AddScheme<AuthenticationSchemeOptions, AuthenticationHandler>("MyAuthScheme", null);

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
                o.SwaggerEndpoint($"/swagger/v1/swagger.json", "AS_WebApi_Projekt v1");
                o.SwaggerEndpoint($"/swagger/v2/swagger.json", "AS_WebApi_Projekt v2");
            });
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
