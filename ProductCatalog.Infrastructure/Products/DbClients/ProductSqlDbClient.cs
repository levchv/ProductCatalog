using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain.Core.Ports.Repositories;
using ProductCatalog.Infrastructure.Products.DbClients.SqlModels;
using Toolbelt.ComponentModel.DataAnnotations;

namespace ProductCatalog.Infrastructure.Products.DbClients
{
    internal class ProductSqlDbClient: DbContext, IUnitOfWork
    {   
		private string connectionString;

		public ProductSqlDbClient(): this(@"Server=.\;")
		{

		}

		public ProductSqlDbClient(string connectionString)
        {
			this.connectionString = connectionString;
        }

        public DbSet<Product> Products { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
				optionsBuilder.UseSqlServer(connectionString);
            }
        }
				
		protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.BuildIndexesFromAnnotations();
    }
    }
}