using ProductCatalog.Domain.Core.DataModels;

namespace ProductCatalog.Domain.Products.InputModels
{
    public sealed class ProductInputModel: IInputModel
    {
		public ProductInputModel() { }

		public string Code { get; set; }
		public string Name { get; set; }
		public double Price { get; set; }
		public string Photo { get; set; }
        
    }
}