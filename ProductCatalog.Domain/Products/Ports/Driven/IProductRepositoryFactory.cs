using ProductCatalog.Domain.Core.Ports.Factories;

namespace ProductCatalog.Domain.Products.Ports.Driven
{
    public interface IProductRepositoryFactory: ISimpleFactory<IProductRepository>
    {
         
    }
}