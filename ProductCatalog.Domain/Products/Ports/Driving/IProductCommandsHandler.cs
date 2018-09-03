using System;
using System.Threading.Tasks;
using ProductCatalog.Domain.Products.InputModels;
using ProductCatalog.Domain.Products.Statuses;

namespace ProductCatalog.Domain.Products.Ports.Driving
{
	public interface IProductCommandsHandler
	{
		Task<bool> DeleteAsync(string id);
		Task<EUpdateProductCommandStatus> UpdateAsync(string id, ProductInputModel value);
		Task<bool> CreateAsync(ProductInputModel product);
	}
}