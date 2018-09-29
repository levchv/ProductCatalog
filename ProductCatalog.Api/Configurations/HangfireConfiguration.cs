using System.Collections.Generic;
using ProductCatalog.Api.Core.Jobs;

namespace ProductCatalog.Api.Configurations
{
	public class HangfireConfiguration
	{
		public bool StartServer { get; set; }
		public IAsyncJob[] AsyncBackgroundJobs { get; set; }
	}
}