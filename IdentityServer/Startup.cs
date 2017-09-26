using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IdentityManagerDotNet.DataLayer;
using IdentityManagerDotNet.DataLayer.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer.Services;
using IdentityServer4.EntityFramework.DbContexts;

namespace IdentityServer
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
            services.AddDbContext<ApplicationDbContext>(ConfigureDbContext);

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

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
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            InitializeDatabase(app);

            app.UseStaticFiles();

            app.UseIdentityServer();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>().Database.Migrate();
            }
        }
    }
}
