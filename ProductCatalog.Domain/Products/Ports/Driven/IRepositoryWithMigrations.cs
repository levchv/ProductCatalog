using System.Threading.Tasks;

namespace ProductCatalog.Domain.Products.Ports.Driven
{
	public interface IRepositoryWithMigrations
	{
		Task MigrateAsync();	
	}
}