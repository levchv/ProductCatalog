using System.Threading.Tasks;
using ProductCatalog.Domain.Core.DataModels;

namespace ProductCatalog.Domain.Core.Ports.Repositories
{
    public interface ISimpleRepository<TEntity, TKey> where TEntity: IEntity
    {
		IUnitOfWork UnitOfWork { get; } 
        void Add(TEntity item);        
        Task UpdateAsync(TEntity item);      
        Task<bool> DeleteAsync(TKey id);
        Task<TEntity> GetAsync(TKey id);
    }
}