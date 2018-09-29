using System.Threading.Tasks;

namespace ProductCatalog.Api.Core.Jobs
{
	public interface IAsyncJob
	{
		Task RunAsync();
	}
}