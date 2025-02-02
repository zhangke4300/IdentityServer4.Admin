using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using Supplyer.API.Configuration;
using Supplyer.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supplyer.API
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
            var supplyApiConfiguration = Configuration.GetSection(nameof(SupplyApiConfiguration)).Get<SupplyApiConfiguration>();
            services.AddSingleton(supplyApiConfiguration);
            services.AddControllers();
            services.AddAuthConfig(Configuration);
            //services.AddAuthOpidConfig(Configuration);
            services.AddSwaggerConfig(supplyApiConfiguration);
            services.AddTransient<IIdentityService, IdentityService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1");
                    c.OAuthClientId("SupplySwagger");
                    c.OAuthAppName("Supply - Swagger");
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            IdentityModelEventSource.ShowPII = true;

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
