using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Jwt_Web_Client_Sample.Models;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Jwt_Web_Client_Sample.Controllers;

namespace Jwt_Web_Client_Sample
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment environment)
        {   
            var builder = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSession();

            services.AddDbContext<IdentityDbContext>();
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //        .AddCookie(options =>
            //        {
            //            options.LoginPath = "/Account/Login";
            //            options.LogoutPath = "/Account/Logout";
            //        });
            //.AddCookie(o=> o.Cookie = new CookieBuilder() { Path = "/", Expiration = TimeSpan.FromDays(1), SecurePolicy = CookieSecurePolicy.SameAsRequest, MaxAge = TimeSpan.FromDays(1) })
            //.AddCookie(options => { options.LoginPath = "/Account/Login"; options.LogoutPath = "/Account/Logout"; });

            services.ConfigureApplicationCookie(options => options.LoginPath = AppData.LoginPath);
            services
                .AddAuthentication(o => o.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = AppData.LoginPath;
                    options.LogoutPath = "/Account/Logout";
                });

            services.AddAuthentication()
                .AddFacebook(o =>
                {   
                    o.AppId = Configuration.GetValue<string>("Facebook:AppId");
                    o.AppSecret = Configuration.GetValue<string>("Facebook:AppSecret");
                    o.Events.OnRemoteFailure = AccountController.OnExternalLoginDenial;
                });
            services.AddAuthentication()
                .AddGoogle(o =>
                {
                    o.ClientId = Configuration.GetValue<string>("Google:ClientId");
                    o.ClientSecret = Configuration.GetValue<string>("Google:ClientSecret");
                    o.Events.OnRemoteFailure = AccountController.OnExternalLoginDenial;
                });

            services.AddAuthentication()
                .AddTwitter(o =>
                {
                    o.ConsumerKey = Configuration.GetValue<string>("Twitter:ConsumerKey");
                    o.ConsumerSecret = Configuration.GetValue<string>("Twitter:ConsumerSecret");
                    o.Events.OnRemoteFailure = AccountController.OnExternalLoginDenial;
                });

            AppData.ApiUrl = Configuration.GetValue<string>("apiUrl");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();
            app.UseMvc().UseMvcWithDefaultRoute();
        }
    }
}
