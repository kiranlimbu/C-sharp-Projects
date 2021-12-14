using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jobTrackerAPI.Data;
using jobTrackerAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace jobTrackerAPI
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
            services.AddCors();
            services.AddTransient<IApplicationsRepository, InMemItemsRepository>();

            services.AddDbContext<ApplicationContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "jobTrackerAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "jobTrackerAPI v1"));
            }

            // app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(options => options
                .WithOrigins(new []{"http://localhost:3000", "http://localhost:8080", "http://localhost:4200",})
                .AllowAnyHeader() // port for react.js           // port for View         // port for Angular
                .AllowAnyMethod()
                .AllowCredentials()
            );

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
