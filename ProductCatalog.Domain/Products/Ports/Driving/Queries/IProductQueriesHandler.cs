using System.Threading.Tasks;
using ProductCatalog.Domain.Products.Ports.Driving.Queries.ViewModels;

namespace ProductCatalog.Domain.Core.Ports.Driving.Queries
{
	public interface IProductQueriesHandler
	{
		Task<ProductViewModel> GetAsync(string id);
		Task<ProductViewModel[]> GetAllAsync();
		Task<ProductViewModel[]> SearchAsync(string search);
	}
}