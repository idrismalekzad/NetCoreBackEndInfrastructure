using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WebApiExample.Data.Configs;
using WebApiExample.Data.Entities;

namespace WebApiExample.Data
{
    public class WebApiDbContextOracle : DbContext
    {
        private readonly IServiceProvider _serviceProvider;
        public WebApiDbContextOracle(DbContextOptions<WebApiDbContextOracle> options, IServiceProvider serviceProvider) : base(options)
        {
            _serviceProvider = serviceProvider;
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        public WebApiDbContextOracle(IServiceProvider serviceProvider) : base()
        {
            _serviceProvider = serviceProvider; 
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        public DbSet<WebServiceUserEntity> WebServiceUser { get; set; }

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
                using (var scope = _serviceProvider.CreateScope())
                {
                    var connectionStringConfig = scope.ServiceProvider.GetRequiredService<IOptions<ConnectionStringConfig>>().Value;

                    var oracleConnectionString = getConnectionStringOracle();

                    if (connectionStringConfig.OracleActivaityStatus == "true")
                    {
                        optionsBuilder.UseOracle(oracleConnectionString);/*, options =>*/
                        //{
                        //    options.MigrationsAssembly("Migrations.Oracle");
                        //});
                    }
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
