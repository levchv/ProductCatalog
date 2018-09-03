using System;
using System.Threading.Tasks;
using ProductCatalog.Domain.Core.Ports.Repositories;
using ProductCatalog.Domain.Products.Entities;
using ProductCatalog.Domain.Products.Ports.Driven;
using ProductCatalog.Infrastructure.Products.DbClients;

namespace ProductCatalog.Infrastructure.Products.Adapters.Repositories
{
    public class ProductSqlRepository: IProductRepository
    {
        private ProductSqlDbClient repository;

		public ProductSqlRepository(string connectionString)
        {
			if (string.IsNullOrWhiteSpace(connectionString))
				throw new ArgumentException("Connection string is empty");
				
			this.repository = new ProductSqlDbClient(connectionString);
        }

		public IUnitOfWork UnitOfWork => repository;

		public void Add(Product item)
		{
			var product = new DbClients.SqlModels.Product() { Code = item.GetCode(), Name = item.GetName(), Price = item.GetPrice(), Photo = item.GetPhoto(), LastUpdated = DateTime.UtcNow };
			repository.Add(product);
		}

		public Task<bool> CheckIfProductCodeUniqueAsync(string code)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> DeleteAsync(string id)
		{
			var product = await repository.FindAsync<DbClients.SqlModels.Product>(id);
			if (product == null)
				return false;

			repository.Remove(product);
			return true;
		}

		public async Task<Product> GetAsync(string id)
		{
			var product = await repository.FindAsync<DbClients.SqlModels.Product>(id);
			return new Product(product.Id.ToString(), product.Code, product.Name, product.Price, product.Photo);
		}

		public async Task UpdateAsync(Product item)
		{
			var product = await repository.FindAsync<DbClients.SqlModels.Product>(item.GetId());
			product.Code = item.GetCode();
			product.Name = item.GetName();
			product.Price = item.GetPrice();
			product.Photo = item.GetPhoto();
			product.LastUpdated = DateTime.UtcNow;
		}
	}
}