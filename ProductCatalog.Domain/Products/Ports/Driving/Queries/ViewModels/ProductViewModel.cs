using System;
using ProductCatalog.Domain.Core.DataModels;

namespace ProductCatalog.Domain.Products.Ports.Driving.Queries.ViewModels
{
    public sealed class ProductViewModel: IViewModel
    {
		private ProductViewModel() { }

		public ProductViewModel(string id, string code, string name, double price, string photo, DateTime lastUpdated)
		{
			Id = id;
			Code = code;
			Name = name;
			Price = price;
			Photo = photo;
			LastUpdated = lastUpdated;
		}

		public string Id { get; }
		public string Code { get; }
		public string Name { get; }
		public double Price { get; }
		public string Photo { get; }
		public DateTime LastUpdated { get; }
	}
}