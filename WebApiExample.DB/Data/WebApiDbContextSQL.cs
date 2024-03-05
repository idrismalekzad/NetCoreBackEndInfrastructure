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
    public class WebApiDbContextSQL : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        private readonly IServiceProvider _serviceProvider;
        public WebApiDbContextSQL(DbContextOptions<WebApiDbContextSQL> options, IServiceProvider serviceProvider) : base(options)
        {
            _serviceProvider = serviceProvider;
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        public WebApiDbContextSQL(IServiceProvider serviceProvider) : base()
        {
            _serviceProvider = serviceProvider;
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        public DbSet<ApplicationUser> ApplicationUsers{ get; set; }

        private static string getConnectionStringSQLServer()
        {
            var environmentName =
              Environment.GetEnvironmentVariable(
                  "ASPNETCORE_ENVIRONMENT");

            var config = new ConfigurationBuilder().AddJsonFile("appsettings" + (String.IsNullOrWhiteSpace(environmentName) ? "" : "." + environmentName) + ".json", false).Build();
            
            return config.GetConnectionString("DefaultConnectionSQLServer");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = getConnectionStringSQLServer();

                using (var scope = _serviceProvider.CreateScope())
                {
                    var connectionStringConfig = scope.ServiceProvider.GetRequiredService<IOptions<ConnectionStringConfig>>().Value;
                    if (connectionStringConfig.SQLServerActivaityStatus == "true")
                    {
                        optionsBuilder.UseSqlServer(connectionString);/*, options =>*/
                        //options.MigrationsAssembly("Migrations.SQL"));
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
