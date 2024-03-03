using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using WebApiExample.Data.Configs;
using WebApiExample.Data.Entities;

namespace WebApiExample.Data
{
    public class WebApiDbContext : DbContext
    {
        private readonly ConnectionStringConfig _connectionStringConfig;
        public WebApiDbContext(DbContextOptions<WebApiDbContext> options, IOptions<ConnectionStringConfig> connectionStringConfig) : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
            _connectionStringConfig = connectionStringConfig.Value;
        }

        public WebApiDbContext(IOptions<ConnectionStringConfig> connectionStringConfig) : base()
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
            _connectionStringConfig = connectionStringConfig.Value;
        }

        private static string getConnectionStringSQLServer()
        {
            var environmentName =
              Environment.GetEnvironmentVariable(
                  "ASPNETCORE_ENVIRONMENT");

            var config = new ConfigurationBuilder().AddJsonFile("appsettings" + (String.IsNullOrWhiteSpace(environmentName) ? "" : "." + environmentName) + ".json", false).Build();
            
            return config.GetConnectionString("DefaultConnectionSQLServer");
        }
        private static string getConnectionStringOracle()
        {
            var environmentName =
              Environment.GetEnvironmentVariable(
                  "ASPNETCORE_ENVIRONMENT");

            var config = new ConfigurationBuilder().AddJsonFile("appsettings" + (String.IsNullOrWhiteSpace(environmentName) ? "" : "." + environmentName) + ".json", false).Build();

            return config.GetConnectionString("DefaultConnectionOracle");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //var connectionString = getConnectionStringSQLServer();
                //optionsBuilder.UseSqlServer(connectionString);

                // Use Oracle provider
                //var oracleConnectionString = getConnectionStringOracle();
                //optionsBuilder.UseOracle(oracleConnectionString);


                if (_connectionStringConfig.OracleActivaityStatus == "true")
                {
                    var connectionString = getConnectionStringOracle();
                    optionsBuilder.UseOracle(connectionString, options =>
                    {
                        options.MigrationsAssembly("Migrations.Oracle");
                    });
                }
                else if (_connectionStringConfig.SQLServerActivaityStatus == "true")
                {
                    var connectionString = getConnectionStringSQLServer();
                    optionsBuilder.UseSqlServer(connectionString, options =>
                    options.MigrationsAssembly("Migrations.SQL"));
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
