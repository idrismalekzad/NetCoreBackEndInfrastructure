using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Reflection;
using System.Text;
using WebApiExample.Data;
using WebApiExample.Data.Configs;
using WebApiExample.Data.Entities;
using WebApiExample.Infrastructure.Initializer;
using WebApiExample.Infrastructure.UnitOfWork;
using WebApiExample.Services;
using WebApiExample.Services.JWT;
using WebApiExample.Services.JWT.Middleware;

var builder = WebApplication.CreateBuilder(args);

#region ConnectionStringHelper
static string getConnectionStringSQLServer()
{
    var environmentName =
      Environment.GetEnvironmentVariable(
          "ASPNETCORE_ENVIRONMENT");

    var config = new ConfigurationBuilder().AddJsonFile("appsettings" + (String.IsNullOrWhiteSpace(environmentName) ? "" : "." + environmentName) + ".json", false).Build();

    return config.GetConnectionString("DefaultConnectionSQLServer");
}
static string getConnectionStringOracle()
{
    var environmentName =
      Environment.GetEnvironmentVariable(
          "ASPNETCORE_ENVIRONMENT");

    var config = new ConfigurationBuilder().AddJsonFile("appsettings" + (String.IsNullOrWhiteSpace(environmentName) ? "" : "." + environmentName) + ".json", false).Build();

    return config.GetConnectionString("DefaultConnectionOracle");
}
#endregion
// Add services to the container.

#region JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        //ValidIssuer = builder.Configuration["JWTBearerSettings:Issuer"],
        //ValidAudience = builder.Configuration["JWTBearerSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTBearerSettings:Key"])),
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});
#endregion

builder.Services.AddAuthorization();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiExample", Version = "v1" });

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = @"Enter 'Bearer' [space] and your token",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    },
                    Scheme="oauth2",
                    Name="Bearer",
                    In=ParameterLocation.Header
                },
                new List<string>()
            }

        });
    });

#region OracleRegistration
builder.Services.AddDbContext<WebApiDbContextOracle>((serviceProvider, options) =>
{
   
});
#endregion

builder.Services.Configure<ConnectionStringConfig>(builder.Configuration.GetSection("ConnectionStrings"));

#region SQLServerRegistration
builder.Services.AddDbContext<WebApiDbContextSQL>((serviceProvider, options) =>
{
    
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<WebApiDbContextSQL>();
#endregion

builder.Services.AddScoped<IWebApiUnitOfWorkAsync, WebApiUnitOfWorkAsync>();
builder.Services.AddScoped<WebServiceUserService, WebServiceUserService>();
builder.Services.AddScoped<IJWTService, JWTService>();
builder.Services.AddScoped<IDbInitializer, DbInitializer>();
// End Infrastructure Implemention

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var connectionStringConfig = scope.ServiceProvider.GetRequiredService<IOptions<ConnectionStringConfig>>().Value;

    if (connectionStringConfig.OracleActivaityStatus == "true")
    {
        var dataContextoracle = scope.ServiceProvider.GetRequiredService<WebApiDbContextOracle>();
        dataContextoracle.Database.Migrate();
    }
    if (connectionStringConfig.SQLServerActivaityStatus == "true")
    {
        var dataContextsql = scope.ServiceProvider.GetRequiredService<WebApiDbContextSQL>();
        dataContextsql.Database.Migrate();
    }

    var f = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
    //f.Initialize();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<JWTMiddleware>();
app.MapControllers();

app.Run();




















// void testconnecrtion()
//{
//    Console.WriteLine("Starting...");

//    // Replace with your actual connection string
//    using (var _db = new OracleConnection("User Id=hswitch;Password=hswitch;Data Source=10.9.12.40:1521/odb;"))
//    {
//        try
//        {
//            Console.WriteLine("Opening connection...");
//            _db.Open();
//            Console.WriteLine("Connected successfully!");

//            // Retrieve server version (optional)
//            var serverVersion = _db.ServerVersion;
//            Console.WriteLine($"Server version: {serverVersion}");
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Error: {ex.Message}");
//        }
//    }

//    Console.WriteLine("Press any key to exit...");
//}

//testconnecrtion();