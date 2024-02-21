using WebApiExample.Data.Entities;

namespace WebApiExample.Services
{
    public interface IJWTService
    {
        Task<string> GenerateToken(ApplicationUser user);
    }
}
