using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain.Products.Ports.Driven;
using ProductCatalog.Domain.Products.ViewModels;
using ProductCatalog.Infrastructure.Products.DbClients;

namespace ProductCatalog.Infrastructure.Products.Adapters.Repositories
{
    internal class ProductReadOnlySqlRepository: IProductReadOnlyRepository
    {
        private ProductSqlDbClient repository;

		public ProductReadOnlySqlRepository(string connectionString)
        {
			if (string.IsNullOrWhiteSpace(connectionString))
				throw new ArgumentException("Connection string is empty");
				
			this.repository = new ProductSqlDbClient(connectionString);
        }
		
		public async Task<ProductViewModel[]> GetAllAsync()
		{
			return await repository.Products
				.Select(i => new ProductViewModel(i.Id.ToString(), i.Code, i.Name, i.Price, i.Photo, i.LastUpdated))
				.ToArrayAsync();
		}

		public async Task<ProductViewModel> GetAsync(string id)
        {
            if (!int.TryParse(id, out var intId))
                throw new ArgumentException("Id should be integer number");

            var product = await repository.Products.FindAsync(intId);
            if (product == null)
                return null;

			return new ProductViewModel(product.Id.ToString(), product.Code, product.Name, product.Price, product.Photo, product.LastUpdated);
		}

		public async Task<ProductViewModel[]> SearchAsync(string search)
		{
			search = (search ?? string.Empty).ToUpper();

			return await repository.Products
				.Where(i => i.Code.ToUpper().Contains(search) || i.Name.ToUpper().Contains(search))
				.Select(i => new ProductViewModel(i.Id.ToString(), i.Code, i.Name, i.Price, i.Photo, i.LastUpdated))
				.ToArrayAsync();
		}
    }
}