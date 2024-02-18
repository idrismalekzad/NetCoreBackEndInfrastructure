using System;

namespace BackEndInfrastructure.Infrastructure.Exceptions
{
    public class ServiceStorageException : ServiceException
    {
        public ServiceStorageException(string message, Exception innerException) : base(message, innerException, 1)
        {

        }
    }
}
