using System;
using ProductCatalog.Domain.Products.Ports.Driven;
using ProductCatalog.Infrastructure.Products.Adapters.Repositories;

namespace ProductCatalog.Infrastructure.Products.Adapters.Factories
{
	public class ProductRepositoryFactory : IProductRepositoryFactory
	{
		private string connectionString;

		public ProductRepositoryFactory(string connectionString)
		{
			if (string.IsNullOrWhiteSpace(connectionString))
				throw new ArgumentException("Connection string is empty");

			this.connectionString = connectionString;	
		}
		public IProductRepository Get()
		{
			return new ProductSqlRepository(connectionString);
		}
	}
}