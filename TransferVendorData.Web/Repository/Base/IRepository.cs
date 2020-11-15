using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TransferVendorData.Web.Repository.Base
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);

        bool RecordExists(Expression<Func<TEntity, bool>> predicate);
    }
}
