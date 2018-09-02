using System;
using System.Threading.Tasks;
using ProductCatalog.Domain.Products.Ports.Driving.Commands.InputModels;
using ProductCatalog.Domain.Products.Ports.Driving.Commands.Statuses;

namespace ProductCatalog.Domain.Core.Ports.Driving.Commands
{
	public interface IProductCommandsHandler
	{
		Task<bool> DeleteAsync(string id);
		Task<EUpdateProductCommandStatus> UpdateAsync(string id, ProductInputModel value);
		Task<bool> CreateAsync(ProductInputModel product, out string id);
	}
}