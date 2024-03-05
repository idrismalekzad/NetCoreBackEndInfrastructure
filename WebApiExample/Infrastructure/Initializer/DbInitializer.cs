using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using WebApiExample.Data;
using WebApiExample.Data.Entities;

namespace WebApiExample.Infrastructure.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly WebApiDbContextOracle _dbContext;

        public DbInitializer(WebApiDbContextOracle dbContext,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {

            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
        }
        public void Initialize()
        {
            if (_roleManager.FindByNameAsync(GlobalRoles.Admin).Result == null)
            {
                _roleManager.CreateAsync(new IdentityRole(GlobalRoles.Admin)).GetAwaiter().GetResult();
            }
            if (_userManager.FindByNameAsync("Admin")?.Result == null)
            {
                ApplicationUser adminUser = new ApplicationUser()
                {
                    UserName = "Admin",
                    Email = "admin@gmail.com",
                    EmailConfirmed = true,
                    FirstName = "کاربر",
                    LastName = "مدیر سیستم"
                };

                _userManager.CreateAsync(adminUser, "Admin123*").GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(adminUser, GlobalRoles.Admin).GetAwaiter().GetResult();
                _userManager.AddClaimsAsync(adminUser, new Claim[]
                {
                new Claim(JwtClaimTypes.Subject,adminUser.Email),
                new Claim(JwtClaimTypes.Email,adminUser.Email),
                }).GetAwaiter().GetResult();
            }
        }
    }
}
