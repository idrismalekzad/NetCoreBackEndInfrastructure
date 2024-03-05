using BackEndInfrastructure.Enums;
using BackEndInfrastructure.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using WebApiExample.Data;
using WebApiExample.Data.Entities;
using WebApiExample.DB.Data.Domain;
using WebApiExample.Infrastructure.Repositories.Interfaces;

namespace WebApiExample.Infrastructure.Repositories
{
    public class WebServiceUserRepository : RepositoryAsync<WebServiceUserEntity, WebServiceUser, int>, IWebServerUserRepository
    {
        //private readonly WebApiDbContextOracle _webApiDbContext;
        public WebServiceUserRepository(WebApiDbContextOracle dbContext) : base(dbContext, DataBase.Oracle)
        {
            //_webApiDbContext = dbContext;
        }
    }
}
