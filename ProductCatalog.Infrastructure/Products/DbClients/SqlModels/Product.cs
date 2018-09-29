using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Toolbelt.ComponentModel.DataAnnotations.Schema;

namespace ProductCatalog.Infrastructure.Products.DbClients.SqlModels
{
    internal class Product
    {
		public int Id { get; set; }
		[Required]
		[MaxLength(50)]
		[Index(IsUnique=true)]
		public string Code { get; set; }
		[Required]
		[MaxLength(255)]
		public string Name { get; set; }
		public double Price { get;  set; }
		public string Photo { get;  set; }
		public DateTime LastUpdated { get; set; }
        
    }
}