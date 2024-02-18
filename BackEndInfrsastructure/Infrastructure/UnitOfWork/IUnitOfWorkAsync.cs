using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEndInfrastructure.Infrastructure.UnitOfWork
{
    public interface IUnitOfWorkAsync : IDisposable
    {
        Task<int> CommitAsync();
    }
}
