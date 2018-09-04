using System;
using ProductCatalog.Domain.Products.Ports.Driven;
using ProductCatalog.Infrastructure.Products.Adapters.Repositories;

namespace ProductCatalog.Infrastructure.Products.Adapters.Factories
{
    public class ProductReadOnlyRepositoryFactory
    {
        private string connectionString;

		public ProductReadOnlyRepositoryFactory(string connectionString)
		{
			if (string.IsNullOrWhiteSpace(connectionString))
				throw new ArgumentException("Connection string is empty");

			this.connectionString = connectionString;	
		}
		public IProductReadOnlyRepository Get()
		{
			return new ProductReadOnlySqlRepository(connectionString);
		}
    }
}