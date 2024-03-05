using WebApiExample.Infrastructure.Repositories.Interfaces;

namespace WebApiExample.Infrastructure.UnitOfWork
{
    public interface IWebApiUnitOfWorkAsync
    {
        IWebServerUserRepository WebServerUser { get; }
    }
}
