using System.Threading.Tasks;
using ProductCatalog.Domain.Core.Ports.Repositories;
using ProductCatalog.Domain.Products.Entities;

namespace ProductCatalog.Domain.Products.Ports.Driven
{
	public interface IProductRepository : ISimpleRepository<Product, string>, IRepositoryWithMigrations
	{
		Task<bool> CheckIfProductCodeUniqueAsync(string code);
		Task<bool> CheckIfProductCodeUniqueAsync(string id, string code);
	}
}