using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProductCatalog.Domain.Core.Ports.Repositories
{
    public interface IUnitOfWork: IDisposable
    {        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}