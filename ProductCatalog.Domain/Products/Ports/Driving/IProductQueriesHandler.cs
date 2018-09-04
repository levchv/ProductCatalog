using System.Threading.Tasks;
using ProductCatalog.Domain.Products.ViewModels;

namespace ProductCatalog.Domain.Products.Ports.Driving
{
	public interface IProductQueriesHandler
	{
		Task<ProductViewModel> GetAsync(string id);
		Task<ProductViewModel[]> GetAllAsync();
		Task<ProductViewModel[]> SearchAsync(string search);
	}
}