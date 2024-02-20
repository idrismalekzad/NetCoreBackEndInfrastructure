using BackEndInfrastructure.DynamicLinqCore;
using BackEndInfrsastructure.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BackEndInfrastructure.Infrastructure.Repository
{
    public abstract class RepositoryAsync<DBModelEntity, DomainModelEntity, PrimaryKeyType> : IRepositoryAsync<DomainModelEntity, PrimaryKeyType>
       where DomainModelEntity : Model<PrimaryKeyType>
       where DBModelEntity : DomainModelEntity
       where PrimaryKeyType : struct

    {
        private readonly DbSet<DomainModelEntity> _entity;
        private readonly DbContext _dbContext;
        public RepositoryAsync(DbContext dbContext)
        {
            _dbContext = dbContext;
            _entity = _dbContext.Set<DomainModelEntity>();

        }
        /// <summary>
        /// Get All ITems Of DomainModelEntity IN IReadOnlyList Type 
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyList<DomainModelEntity>> AllItemsAsync()
        {
            return await _entity.ToListAsync();
        }

        /// <summary>
        /// Get All ITems Of DomainModelEntity IN IEnumerable Type With Handling Paging - Sorting - Filter
        /// </summary>
        /// <returns></returns>
        public virtual async Task<LinqDataResult<DomainModelEntity>> AllItemsAsync(LinqDataRequest request)
        {
            return await _dbContext.Set<DBModelEntity>().ToLinqDataResultAsync<DomainModelEntity>(request.Take, request.Skip, request.Sort, request.Filter);
        }

        public virtual async Task DeleteAsync(DomainModelEntity item)
        {
            if (item is DBModelEntity)
            {
                _entity.Remove(item as DBModelEntity);
            }
        }

        public virtual Task<DomainModelEntity?> FirstOrDefaultAsync(Expression<Func<DomainModelEntity, bool>> predicate)
        {
            return _entity.FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<DomainModelEntity?> GetByIdAsync(PrimaryKeyType id) => await _entity.FindAsync(id);

        public virtual async Task<DomainModelEntity> InsertAsync(DomainModelEntity item)
        {
            if (item is DBModelEntity)
            {
                return _entity.Add(item as DBModelEntity).Entity;
            }
            else
            {
                return default(DomainModelEntity);
            }
        }

        public Task UpdateAsync(DomainModelEntity item)
        {
            throw new NotImplementedException();
        }
    }
}
