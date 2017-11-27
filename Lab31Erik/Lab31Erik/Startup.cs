using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Lab31Erik.Models;
using Lab31Erik.Data;
using Microsoft.AspNetCore.Identity;

namespace Lab31Erik
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddAuthorization(options =>
           options.AddPolicy("Admin Only", policy => policy.RequireClaim("Administrator")));

            services.AddAuthorization(options =>
            options.AddPolicy("Registered User", policy => policy.RequireClaim("RegisteredUser")));


            services.AddDbContext<Lab31ErikContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("Lab31ErikContext")));

            services.AddDbContext<ApplicationDbContext>(options =>
                   options.UseSqlServer(Configuration.GetConnectionString("Lab31ErikContext")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();

            app.Run(async (context) =>
            {
                context.Response.Redirect("/Accounts/AccessDenied", false);
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
