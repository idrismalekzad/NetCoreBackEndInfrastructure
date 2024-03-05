using BackEndInfrastructure.Infrastructure.UnitOfWork;
using WebApiExample.Data;
using WebApiExample.Infrastructure.Repositories;
using WebApiExample.Infrastructure.Repositories.Interfaces;

namespace WebApiExample.Infrastructure.UnitOfWork
{
    public class WebApiUnitOfWorkAsync : UnitOfWorkAsync<WebApiDbContextOracle>, IWebApiUnitOfWorkAsync
    {
        public WebApiUnitOfWorkAsync(IServiceProvider serviceProvider) : base(new WebApiDbContextOracle(serviceProvider))
        {
            WebServerUser = new WebServiceUserRepository(base._dbContext);
        }

        public IWebServerUserRepository WebServerUser { get; private set; }
    }
}
