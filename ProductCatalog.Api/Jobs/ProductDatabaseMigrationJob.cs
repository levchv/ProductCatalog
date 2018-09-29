using System;
using System.Threading.Tasks;
using ProductCatalog.Api.Core.Jobs;
using ProductCatalog.Domain.Core.Ports.Shared;
using ProductCatalog.Domain.Products.Ports.Driven;

namespace ProductCatalog.Api.Jobs
{
	public class ProductDatabaseMigrationJob: IAsyncJob
	{
		private IProductRepository repository;
		private ILogger logger;

		public ProductDatabaseMigrationJob(IProductRepository repository, ILogger logger)
		{
			this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task RunAsync()
		{
			try
			{
				this.logger.LogInfo("Migrations is started");
				await this.repository.MigrateAsync();
			}
			catch (Exception exp)
			{
				this.logger.LogException(exp);
				throw;
			}
			finally
			{
				this.logger.LogInfo("Migrations is finished");
			}
		}
	}
}