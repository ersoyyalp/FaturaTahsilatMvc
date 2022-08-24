using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using ProjectAspNetCore.Contexts;
using ProjectAspNetCore.Entities;
using ProjectAspNetCore.Interfaces;
using ProjectAspNetCore.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectAspNetCore
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ProjectContext>();

            services.AddHttpContextAccessor();

            services.AddIdentity<AppUser, AppRole>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequiredLength = 1;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
            })
                .AddRoles<AppRole>()
                .AddEntityFrameworkStores<ProjectContext>();


            services.ConfigureApplicationCookie(opt =>
            {
                opt.LoginPath = new PathString("/Home/GirisYap");
                opt.LogoutPath = new PathString("/Home/SignOut");
                opt.Cookie = new CookieBuilder
                {
                    Name = "FaturaSistem",
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    SecurePolicy = CookieSecurePolicy.SameAsRequest, //Always
                };
                opt.SlidingExpiration = true;
                opt.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            });
            //dependency ýnjection
            services.AddScoped<ISepetRepository, SepetRepository>();
            services.AddScoped<IFaturaKategoriRepository, KategoriRepository>();
            services.AddScoped<IFaturaBilgiRepository, FaturaBilgiRepository>();
            services.AddScoped<IFaturaBilgiKategoriRepository, FaturaBilgiKategoriRepository>();
            services.AddSession();

            services.AddControllersWithViews();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment
            env)
        {

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            app.UseStatusCodePagesWithReExecute("/Home/NotFound", "?code={0}");

            app.UseExceptionHandler("/Error");

            app.UseRouting();


            app.UseStaticFiles();
            app.UseSession();

            app.UseAuthentication();//kimlik doðrulamasý
            app.UseAuthorization();//kimlik yetkilendirilmesi

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "areas", pattern: "{area}/{Controller=Home}/{Action=Index}/{id?}");
                endpoints.MapControllerRoute(name: "default", pattern: "{Controller=Home}/{Action=Register}/{id?}");


            });
        }
    }
}
