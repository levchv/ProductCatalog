namespace ProductCatalog.Domain.Core.Ports.Factories
{
    public interface ISimpleFactory<T>
    {
         T Get();
    }
}