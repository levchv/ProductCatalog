using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain.Core.Ports.Repositories;
using ProductCatalog.Domain.Products.Entities;
using ProductCatalog.Domain.Products.Ports.Driven;
using ProductCatalog.Infrastructure.Products.DbClients;

namespace ProductCatalog.Infrastructure.Products.Adapters.Repositories
{
    internal class ProductSqlRepository: IProductRepository
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

		public async Task<bool> CheckIfProductCodeUniqueAsync(string id, string code)
		{
			if (!int.TryParse(id, out var intId))
				throw new ArgumentException("Id should be integer number");

			return await this.repository.Products.FirstOrDefaultAsync(i => i.Id != intId && i.Code == code) == null;
		}

		public async Task<bool> CheckIfProductCodeUniqueAsync(string code)
		{
			return await this.repository.Products.FirstOrDefaultAsync(i => i.Code == code) == null;
		}

		public async Task<bool> DeleteAsync(string id)
		{
			if (!int.TryParse(id, out var intId))
				throw new ArgumentException("Id should be integer number");

			var product = await repository.FindAsync<DbClients.SqlModels.Product>(intId);
			if (product == null)
				return false;

			repository.Remove(product);
			return true;
		}

		public async Task<Product> GetAsync(string id)
		{
			if (!int.TryParse(id, out var intId))
				throw new ArgumentException("Id should be integer number");

			var product = await repository.FindAsync<DbClients.SqlModels.Product>(intId);
			return new Product(product.Id.ToString(), product.Code, product.Name, product.Price, product.Photo);
		}

		public async Task UpdateAsync(Product item)
		{
			if (!int.TryParse(item.GetId(), out var intId))
				throw new ArgumentException("Id should be integer number");

			var product = await repository.FindAsync<DbClients.SqlModels.Product>(intId);
			product.Code = item.GetCode();
			product.Name = item.GetName();
			product.Price = item.GetPrice();
			product.Photo = item.GetPhoto();
			product.LastUpdated = DateTime.UtcNow;
		}
	}
}