using System;
using System.ComponentModel.DataAnnotations;

namespace ProductCatalog.Infrastructure.Products.DbClients.SqlModels
{
    internal class Product
    {
		public int Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public double Price { get;  set; }
		public string Photo { get;  set; }
		public DateTime LastUpdated { get; set; }
        
    }
}