using BackEndInfrastructure.DynamicLinqCore;
using BackEndInfrastructure.Infrastructure.Exceptions;
using BackEndInfrastructure.Infrastructure.Service;
using WebApiExample.DB.Data.Domain;
using WebApiExample.Infrastructure.UnitOfWork;

namespace WebApiExample.Services
{
    public class WebServiceUserService : StorageBusinessService<WebServiceUser, int>
    {
        private readonly IWebApiUnitOfWorkAsync _webApiUnitOfWorkAsync;
        /// <summary>
        /// LogBaseID of the Service Is 1000
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="webApiUnitOfWorkAsync"></param>
        public WebServiceUserService(ILogger<WebServiceUser> logger, IWebApiUnitOfWorkAsync webApiUnitOfWorkAsync) : base(logger, 1000)
        {
            _webApiUnitOfWorkAsync = webApiUnitOfWorkAsync;
        }

        public override Task<int> AddAsync(WebServiceUser item)
        {
            throw new NotImplementedException();
        }

        public override async Task<LinqDataResult<WebServiceUser>> ItemsAsync(LinqDataRequest request)
        {
            try
            {
                var f = await _webApiUnitOfWorkAsync.WebServerUser.AllItemsAsync(request);
                LogRetrieveMultiple(null, request);
                return f;
            }
            catch (Exception ex)
            {
                LogRetrieveMultiple(null, request, ex);
                throw new ServiceStorageException("Error retrieving the WebServiceUser", ex);
            }
        }

        public override Task ModifyAsync(WebServiceUser item)
        {
            throw new NotImplementedException();
        }

        public override Task RemoveByIdAsync(int ID)
        {
            throw new NotImplementedException();
        }

        public override Task<WebServiceUser> RetrieveByIdAsync(int ID)
        {
            throw new NotImplementedException();
        }


        protected override Task ValidateOnAddAsync(WebServiceUser item)
        {
            throw new NotImplementedException();
        }

        protected override Task ValidateOnModifyAsync(WebServiceUser recievedItem, WebServiceUser storageItem)
        {
            throw new NotImplementedException();
        }
    }
}
