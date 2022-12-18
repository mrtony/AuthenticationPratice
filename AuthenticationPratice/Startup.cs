using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace AuthenticationPratice
{
    public class Startup
    {
        private readonly IHostEnvironment _environment;

        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            Configuration = configuration;
            _environment = environment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var isLocal = Convert.ToBoolean(Environment.GetEnvironmentVariables()["ASPNETCORE_IS_LOCAL"], System.Globalization.CultureInfo.CurrentCulture);
            // 對全部的controll加入權限控管
            services.AddControllersWithViews(option => option.Filters.Add(new AuthorizeFilter()));

            // 本地開發不需要data protection (可以考慮在appSetting中設定)
            if (isLocal)
            {
                services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie();
            }
            else
            {
                // 在虛擬主機建立data protection (路徑應該可以再優化)
                var keysFolder = Path.Combine(_environment.ContentRootPath, "protection6");
                var parent = Directory.GetParent(keysFolder);
                var path = Path.Combine(parent.Parent.FullName, "protection6");
                services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(path))
                .SetApplicationName("SharedCookieApp");

                services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.Cookie.Name = ".AspNet.SharedCookie";
                        options.Cookie.Domain = ".coding-hub.be";
                    });

                // [TODO][main site拿掉是不會有問題. 要試sub domain site]
                //services.ConfigureApplicationCookie(options =>
                //{
                //    options.Cookie.Domain = ".coding-hub.be";
                //    options.Cookie.Name = ".AspNet.SharedCookie";
                //});
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
