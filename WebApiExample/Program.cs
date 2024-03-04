using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Configuration;
using System.Reflection;
using System.Text;
using WebApiExample.Data;
using WebApiExample.Data.Configs;
using WebApiExample.Data.Entities;
using WebApiExample.Infrastructure.Initializer;
using WebApiExample.Services.JWT;
using WebApiExample.Services.JWT.Middleware;

var builder = WebApplication.CreateBuilder(args);
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
// Add services to the container.

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

//Configuration

builder.Services.Configure<ConnectionStringConfig>(builder.Configuration.GetSection("ConnectionStrings"));
//

// Infrastructure Implemention
builder.Services.AddDbContext<WebApiDbContext>((serviceProvider, options) =>
{
    var connectionStringConfig = serviceProvider.GetRequiredService<IOptions<ConnectionStringConfig>>();

    if (connectionStringConfig.Value.OracleActivaityStatus == "true")
    {
        var connectionString = getConnectionStringOracle();
        options.UseOracle(connectionString, options =>
        {
            options.MigrationsAssembly("Migrations.Oracle");
        });
    }
    else if (connectionStringConfig.Value.SQLServerActivaityStatus == "true")
    {
        var connectionString = getConnectionStringSQLServer();
        options.UseSqlServer(connectionString, options =>
        options.MigrationsAssembly("Migrations.SQL"));
    }
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<WebApiDbContext>();
builder.Services.AddScoped<IJWTService, JWTService>();
builder.Services.AddScoped<IDbInitializer, DbInitializer>();
// End Infrastructure Implemention

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<WebApiDbContext>();
    dataContext.Database.Migrate();

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
