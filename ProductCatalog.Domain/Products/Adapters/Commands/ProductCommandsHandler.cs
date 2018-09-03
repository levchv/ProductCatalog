using System;
using System.Threading.Tasks;
using ProductCatalog.Domain.Products.Entities;
using ProductCatalog.Domain.Products.Ports.Driven;
using ProductCatalog.Domain.Products.Ports.Driving;
using ProductCatalog.Domain.Products.InputModels;
using ProductCatalog.Domain.Products.Statuses;

namespace ProductCatalog.Domain.Products.Adapters.Commands
{
    public class ProductCommandsHandler: IProductCommandsHandler
    {
		private IProductRepositoryFactory repositoryFactory;

		public ProductCommandsHandler(IProductRepositoryFactory repositoryFactory)
		{
			this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
		}

		public async Task<bool> CreateAsync(ProductInputModel newProduct)
		{
			if (newProduct == null)
				throw new ArgumentNullException(nameof(newProduct));

			var repository = repositoryFactory.Get();

			var isCodeUnique = await repository.CheckIfProductCodeUniqueAsync(newProduct.Code);
			if (!isCodeUnique)
				return false;

			var product = new Product(newProduct.Code, newProduct.Name, newProduct.Price, newProduct.Photo);

			repository.Add(product);
			await repository.UnitOfWork.SaveChangesAsync();

			return true;
		}

		public async Task<bool> DeleteAsync(string id)
		{
			if (string.IsNullOrEmpty(id))
				throw new ArgumentException("Is empty", nameof(id));

			var repository = repositoryFactory.Get();

			var result = await repository.DeleteAsync(id);
			if (!result)
				return false;

			await repository.UnitOfWork.SaveChangesAsync();
			return true;
		}

		public async Task<EUpdateProductCommandStatus> UpdateAsync(string id, ProductInputModel productChanges)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Is empty", nameof(id));
            if (productChanges == null)
				throw new ArgumentNullException(nameof(productChanges));

			var repository = repositoryFactory.Get();
			
			var product = await repository.GetAsync(id);
			if (product == null)
				return EUpdateProductCommandStatus.ProductNotExists;

			var isCodeUnique = await repository.CheckIfProductCodeUniqueAsync(productChanges.Code);
			if (!isCodeUnique)
				return EUpdateProductCommandStatus.FailsBecauseDuplicatedCode;

			product.SetCode(productChanges.Code);
			product.SetName(productChanges.Name);
			product.SetPhoto(productChanges.Photo);
			product.SetPrice(productChanges.Price);
			
			await repository.UpdateAsync(product);
			await repository.UnitOfWork.SaveChangesAsync();

			return EUpdateProductCommandStatus.Success;
		}
	}
}