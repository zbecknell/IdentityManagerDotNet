using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IdentityManagerDotNet.Data;
using IdentityManagerDotNet.Models;
using IdentityManagerDotNet.Services;
using System.Reflection;
using IdentityServer4.EntityFramework.DbContexts;

namespace IdentityManagerDotNet
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<PersistedGrantDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), config => config.MigrationsAssembly(migrationsAssembly)));
            services.AddDbContext<ConfigurationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                config => config.MigrationsAssembly(migrationsAssembly)));

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddConfigurationStore(builder =>
                {
                    builder.ConfigureDbContext = ConfigureDbContext;
                })
                .AddOperationalStore(builder =>
                {
                    builder.ConfigureDbContext = ConfigureDbContext;
                })
                .AddAspNetIdentity<ApplicationUser>();

            services.AddMvc();
        }

        private void ConfigureDbContext(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            dbContextOptionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), options => options.MigrationsAssembly(migrationsAssembly));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentityServer();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
