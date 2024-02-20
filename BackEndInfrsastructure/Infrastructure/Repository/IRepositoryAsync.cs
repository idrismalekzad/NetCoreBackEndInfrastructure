using BackEndInfrastructure.DynamicLinqCore;
using BackEndInfrsastructure.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BackEndInfrastructure.Infrastructure.Repository
{
    public interface IRepositoryAsync<ModelItem, PrimeryKeyType>
        where ModelItem : Model<PrimeryKeyType>
        where PrimeryKeyType : struct
    {
        Task<ModelItem?> GetByIdAsync(PrimeryKeyType id);
        Task<ModelItem?> FirstOrDefaultAsync(Expression<Func<ModelItem, bool>> predicate);


        Task<IReadOnlyList<ModelItem>> AllItemsAsync();
        Task<LinqDataResult<ModelItem>> AllItemsAsync(LinqDataRequest request);


        Task<ModelItem> InsertAsync(ModelItem item);
        Task DeleteAsync(ModelItem item);
        Task UpdateAsync(ModelItem item);
    }
}
