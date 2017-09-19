using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace IdentityManagerDotNet
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();

        //public class ConfigurationDbContextFactory : IDesignTimeDbContextFactory<ConfigurationDbContext>
        //{
        //    public ConfigurationDbContext CreateDbContext(string[] args)
        //    {
        //        IConfigurationRoot configuration = new ConfigurationBuilder()
        //            .SetBasePath(Directory.GetCurrentDirectory())
        //            .AddJsonFile("appsettings.json")
        //            .Build();
        //        var builder = new DbContextOptionsBuilder<ConfigurationDbContext>();
        //        var connectionString = configuration.GetConnectionString("DefaultConnection");

        //    var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
        //        builder.UseSqlite(connectionString, options => options.MigrationsAssembly(migrationsAssembly));
        //        return new ConfigurationDbContext(builder.Options, 
        //                                          new IdentityServer4.EntityFramework.Options.ConfigurationStoreOptions());
        //    }
        //}

        //public class PersistenceDbContextFactory : IDesignTimeDbContextFactory<PersistedGrantDbContext>
        //{
        //    public PersistedGrantDbContext CreateDbContext(string[] args)
        //    {
        //        IConfigurationRoot configuration = new ConfigurationBuilder()
        //            .SetBasePath(Directory.GetCurrentDirectory())
        //            .AddJsonFile("appsettings.json")
        //            .Build();
        //        var builder = new DbContextOptionsBuilder<PersistedGrantDbContext>();
        //        var connectionString = configuration.GetConnectionString("DefaultConnection");

        //        var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
        //        builder.UseSqlite(connectionString, options => options.MigrationsAssembly(migrationsAssembly));
        //        return new PersistedGrantDbContext(builder.Options,
        //                                          new IdentityServer4.EntityFramework.Options.OperationalStoreOptions());
        //    }
        //}
    }
}
