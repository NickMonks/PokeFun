using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Pokemon_API.Controllers;
using Pokemon_API.Exceptions;
using Pokemon_API.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Pokemon_API
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

            services.AddControllers();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pokemon_API", Version = "v1" });
            });

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // HttpClient Services
            services.AddHttpClient();
            services.AddHttpClient("pokemon", c =>
            {
                c.BaseAddress = new Uri(Configuration.GetValue<string>("pokemon"));
            });

            services.AddHttpClient("translation", c =>
            {
                c.BaseAddress = new Uri(Configuration.GetValue<string>("translation"));
            });


            // Register scoped services used for business logic 
            services.AddScoped<IPokemonAsync, PokemonResult>();
            services.AddScoped<IPokemonTranslate, PokemonTranslate>();

            //Register a health check, to check if the application is running 
            services.AddHealthChecks();



        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pokemon_API v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/healthcheck");
            });
        }
    }
}
