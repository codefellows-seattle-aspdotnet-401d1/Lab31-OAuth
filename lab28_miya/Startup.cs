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
using lab28_miya.Models;
using lab28_miya.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lab28_miya
{
    public class Startup
    {
        string _testSecret = null;

        public IConfiguration Configuration
        {
            get;
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            _testSecret = Configuration["MySecret"];

            //Require https
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie("MyCookieLogin", options =>
                options.AccessDeniedPath = new PathString("/Account/Forbidden/"));

            services.AddAuthorization(options =>
            {
                //this is where the policy is created
                options.AddPolicy("Admin Only", policy => policy.RequireRole("Administrator"));
                options.AddPolicy("MinimumYearsInService", policy => policy.Requirements.Add(new MinimumYearsInService()));
                options.AddPolicy("Field Work", policy => policy.RequireRole("CPS Agent"));
                options.AddPolicy("Skill Set", policy => policy.RequireClaim("StartDate"));
            }
            );

            services.AddAuthentication().AddFacebook(facebook =>
            {
                facebook.ClientId = Configuration["MyFacebookAppID"];
                facebook.ClientSecret = Configuration["5c199174958579481067b406ff15c229"];
            }
            );

            services.AddSingleton<IAuthorizationHandler, Has5Years>();

            services.AddMvc();

            services.AddDbContext<lab28_miyaContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("lab28_miyaContext")));

                services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("lab28_miyaContext")));

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

            //I am unclear about whether or not this is needed and when it should be taken out
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("What's my purpose?");
            });
        }
    }
}
