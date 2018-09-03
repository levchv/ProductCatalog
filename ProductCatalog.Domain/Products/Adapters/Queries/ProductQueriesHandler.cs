using System;
using System.Threading.Tasks;
using ProductCatalog.Domain.Products.Ports.Driven;
using ProductCatalog.Domain.Products.Ports.Driving;
using ProductCatalog.Domain.Products.ViewModels;

namespace ProductCatalog.Domain.Products.Adapters.Queries
{
	public class ProductQueriesHandler : IProductQueriesHandler
	{
		private IProductReadOnlyRepository repository;

		public ProductQueriesHandler(IProductReadOnlyRepository repository)
		{
			this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}
		
		public async Task<ProductViewModel[]> GetAllAsync()
		{
			return await repository.GetAllAsync();
		}

		public async Task<ProductViewModel> GetAsync(string id)
		{
			if (string.IsNullOrEmpty(id))
				return null;

			return await repository.GetAsync(id);
		}

		public async Task<ProductViewModel[]> SearchAsync(string search)
		{
			if (string.IsNullOrWhiteSpace(search))
				return new ProductViewModel[0];

			return await repository.SearchAsync(search);
		}
	}
}