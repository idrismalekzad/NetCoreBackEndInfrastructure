using WebApiExample.Data.Entities;

namespace WebApiExample.Services.JWT
{
    public interface IJWTService
    {
        Task<string> GenerateToken(ApplicationUser user);
        Task<ApplicationUser?> GetUserByID(string userId);
    }
}
