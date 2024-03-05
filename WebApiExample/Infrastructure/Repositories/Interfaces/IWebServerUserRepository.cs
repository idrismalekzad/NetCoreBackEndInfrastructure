using BackEndInfrastructure.Infrastructure.Repository;
using WebApiExample.DB.Data.Domain;

namespace WebApiExample.Infrastructure.Repositories.Interfaces
{
    public interface IWebServerUserRepository : IRepositoryAsync<WebServiceUser, int>
    {

    }
}
