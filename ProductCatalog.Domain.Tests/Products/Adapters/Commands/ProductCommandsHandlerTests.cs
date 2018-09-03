using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using ProductCatalog.Domain.Core.Ports.Repositories;
using ProductCatalog.Domain.Products.Adapters.Commands;
using ProductCatalog.Domain.Products.Entities;
using ProductCatalog.Domain.Products.InputModels;
using ProductCatalog.Domain.Products.Ports.Driven;
using ProductCatalog.Domain.Products.Statuses;

namespace ProductCatalog.Domain.Tests.Products.Adapters.Commands
{
    public class ProductCommandsHandlerTests
    {        
         
		[Test]
		public void ClassCouldBeConstruct()
		{
			var repositoryFactory = GetProductRepositoryFactory();
			Assert.DoesNotThrow(() => new ProductCommandsHandler(repositoryFactory));
		}

		[Test]
		public void ConstructorRequiresProductCommandsHandler()
		{	
			Assert.Throws<ArgumentNullException>(() => new ProductCommandsHandler(null));
		}

		[TestCase("234", true)]
		[TestCase("234", false)]
		public async Task DeleteAsync_ValidId_ReturnsRepositoryDeleteResult(string id, bool repositoryResult)
		{
			var repositoryMock = new Mock<IProductRepository>();
			repositoryMock.Setup(repository => repository.DeleteAsync(id)).ReturnsAsync(repositoryResult);
            repositoryMock.Setup(repository => repository.UnitOfWork).Returns(GetUnitOfWork());

            var repositoryFactoryMock =  new Mock<IProductRepositoryFactory>();
			repositoryFactoryMock.Setup(factory => factory.Get()).Returns(repositoryMock.Object);

			var handler = new ProductCommandsHandler(repositoryFactoryMock.Object);
			var result = await handler.DeleteAsync(id);

			Assert.AreEqual(repositoryResult, result);
		}

        [TestCase(null)]
        [TestCase("")]
		public void DeleteAsync_NotValidId_ThrowsArgumentNullException(string id)
		{
			var repositoryFactoryMock = new Mock<IProductRepositoryFactory>();
            repositoryFactoryMock.Setup(factory => factory.Get()).Returns(GetRepository());

            var handler = new ProductCommandsHandler(repositoryFactoryMock.Object);

			Assert.ThrowsAsync<ArgumentException>(async () => await handler.DeleteAsync(id));
		}

        [TestCase("234")]
		public void DeleteAsync_OnRepositoryException_ThrowSameException(string id)
		{
			var exception = new Exception();
			var repositoryMock = new Mock<IProductRepository>();
			repositoryMock.Setup(repository => repository.DeleteAsync(id)).ThrowsAsync(exception);

			var repositoryFactoryMock =  new Mock<IProductRepositoryFactory>();
			repositoryFactoryMock.Setup(factory => factory.Get()).Returns(repositoryMock.Object);

			var handler = new ProductCommandsHandler(repositoryFactoryMock.Object);

			var result = Assert.ThrowsAsync(exception.GetType(), async () => await handler.DeleteAsync(id));			
			Assert.AreSame(exception, result);
		}

		[Test]
		public void CreateAsync_NullInputModel_ThrowsArgumentNullException()
		{
			var repositoryFactoryMock = new Mock<IProductRepositoryFactory>();

			var handler = new ProductCommandsHandler(repositoryFactoryMock.Object);

			Assert.ThrowsAsync<ArgumentNullException>(async () => await handler.CreateAsync(null));
		}

		[Test]
		public async Task CreateAsync_DuplicatedCode_ReturnsFalse()
		{
			var product = GetNewProduct();

			var repositoryMock = new Mock<IProductRepository>();
			repositoryMock.Setup(repository => repository.CheckIfProductCodeUniqueAsync(product.Code)).ReturnsAsync(false);

            var repositoryFactoryMock =  new Mock<IProductRepositoryFactory>();
			repositoryFactoryMock.Setup(factory => factory.Get()).Returns(repositoryMock.Object);

			var handler = new ProductCommandsHandler(repositoryFactoryMock.Object);
			var result = await handler.CreateAsync(product);

			Assert.IsFalse(result);
		}


		[TestCase("", "name", 100.0, "http://asd.sdf.com/asd.jpg")]
		[TestCase(null, "name", 100.0, "http://asd.sdf.com/asd.jpg")]
		[TestCase("code", "", 100.0, "http://asd.sdf.com/asd.jpg")]
		[TestCase("code", null, 100.0, "http://asd.sdf.com/asd.jpg")]
		[TestCase("code", "name", 0, "http://asd.sdf.com/asd.jpg")]
		[TestCase("code", "name", -100.0, "http://asd.sdf.com/asd.jpg")]
		[TestCase("code", "name", 100.0, "asd.34")]
		[TestCase("code", "name", 100.0, "D:\\asd\\sas.jpg")]
		public void CreateAsync_NotValidInputData_ReturnsFalse(string code, string name, double price, string photo)
		{
			var product = new ProductInputModel
			{
				Code = code,
				Name = name,
				Price = price,
				Photo = photo
			};

			var repositoryMock = new Mock<IProductRepository>();
			repositoryMock.Setup(repository => repository.CheckIfProductCodeUniqueAsync(product.Code)).ReturnsAsync(true);

			var repositoryFactoryMock = new Mock<IProductRepositoryFactory>();
			repositoryFactoryMock.Setup(factory => factory.Get()).Returns(repositoryMock.Object);

			var handler = new ProductCommandsHandler(repositoryFactoryMock.Object);
			Assert.ThrowsAsync<ArgumentException>(async () => await handler.CreateAsync(product));
		}

		[Test]
		public async Task CreateAsync_ValidInputData_ReturnsTrue()
		{
			var product = GetNewProduct();

			var repositoryMock = new Mock<IProductRepository>();
			repositoryMock.Setup(repository => repository.CheckIfProductCodeUniqueAsync(product.Code)).ReturnsAsync(true);
            repositoryMock.Setup(repository => repository.UnitOfWork).Returns(GetUnitOfWork());

            var repositoryFactoryMock =  new Mock<IProductRepositoryFactory>();
			repositoryFactoryMock.Setup(factory => factory.Get()).Returns(repositoryMock.Object);

			var handler = new ProductCommandsHandler(repositoryFactoryMock.Object);
			var result = await handler.CreateAsync(product);

			Assert.IsTrue(result);
		}

        [Test]
		public void CreateAsync_OnRepositoryException_ThrowSameException()
		{
			var product = GetNewProduct();
			var exception = new Exception();

			var repositoryMock = new Mock<IProductRepository>();
            repositoryMock.Setup(repository => repository.CheckIfProductCodeUniqueAsync(product.Code)).ReturnsAsync(true);
            repositoryMock.Setup(repository => repository.Add(It.IsAny<Product>())).Throws(exception);

			var repositoryFactoryMock =  new Mock<IProductRepositoryFactory>();
			repositoryFactoryMock.Setup(factory => factory.Get()).Returns(repositoryMock.Object);

			var handler = new ProductCommandsHandler(repositoryFactoryMock.Object);

			var result = Assert.ThrowsAsync(exception.GetType(), async () => await handler.CreateAsync(product));			
			Assert.AreSame(exception, result);
		}

        [TestCase(null)]
        [TestCase("")]
        public void UpdateAsync_NotValidId_ThrowsArgumentNullException(string id)
		{
			var product = GetExistProduct();

			var repositoryFactoryMock = new Mock<IProductRepositoryFactory>();

			var handler = new ProductCommandsHandler(repositoryFactoryMock.Object);

			Assert.ThrowsAsync<ArgumentException>(async () => await handler.UpdateAsync(id, product));
		}

		[Test]
		public void UpdateAsync_NullInputModel_ThrowsArgumentNullException()
		{
			var repositoryFactoryMock = new Mock<IProductRepositoryFactory>();

			var handler = new ProductCommandsHandler(repositoryFactoryMock.Object);

			Assert.ThrowsAsync<ArgumentNullException>(async () => await handler.UpdateAsync("23", null));
		}
		
		[TestCase("23")]
		public async Task UpdateAsync_ProductNotExists_ReturnsProductNotExists(string id)
		{
			var product = GetExistProduct();

			var repositoryMock = new Mock<IProductRepository>();
			repositoryMock.Setup(repository => repository.GetAsync(id)).ReturnsAsync((Product)null);

			var repositoryFactoryMock =  new Mock<IProductRepositoryFactory>();
			repositoryFactoryMock.Setup(factory => factory.Get()).Returns(repositoryMock.Object);
			
			var handler = new ProductCommandsHandler(repositoryFactoryMock.Object);

			var result = await handler.UpdateAsync(id, product);

			Assert.AreEqual(EUpdateProductCommandStatus.ProductNotExists, result);
		}
		
		[TestCase("23")]
		public async Task UpdateAsync_DuplicatedCode_ReturnsFailsBecauseDuplicatedCode(string id)
		{
			var product = GetExistProduct();			
			var productEntity = GetProductEntity(id, product);

			var repositoryMock = new Mock<IProductRepository>();
			repositoryMock.Setup(repository => repository.GetAsync(id)).ReturnsAsync(productEntity);
			repositoryMock.Setup(repository => repository.CheckIfProductCodeUniqueAsync(product.Code)).ReturnsAsync(false);

			var repositoryFactoryMock =  new Mock<IProductRepositoryFactory>();
			repositoryFactoryMock.Setup(factory => factory.Get()).Returns(repositoryMock.Object);
			
			var handler = new ProductCommandsHandler(repositoryFactoryMock.Object);

			var result = await handler.UpdateAsync(id, product);

			Assert.AreEqual(EUpdateProductCommandStatus.FailsBecauseDuplicatedCode, result);
		}
		
		[TestCase("23")]
		public async Task UpdateAsync_ValidInputData_ReturnsSuccess(string id)
		{
			var product = GetExistProduct();			
			var productEntity = GetProductEntity(id, product);

            var repositoryMock = new Mock<IProductRepository>();
            repositoryMock.Setup(repository => repository.CheckIfProductCodeUniqueAsync(product.Code)).ReturnsAsync(true);
            repositoryMock.Setup(repository => repository.GetAsync(id)).ReturnsAsync(productEntity);
            repositoryMock.Setup(repository => repository.UnitOfWork).Returns(GetUnitOfWork());

			var repositoryFactoryMock =  new Mock<IProductRepositoryFactory>();
			repositoryFactoryMock.Setup(factory => factory.Get()).Returns(repositoryMock.Object);
			
			var handler = new ProductCommandsHandler(repositoryFactoryMock.Object);

			var result = await handler.UpdateAsync(id, product);

			Assert.AreEqual(EUpdateProductCommandStatus.Success, result);
		}

		[TestCase("1", "", "name", 100.0, "http://asd.sdf.com/asd.jpg")]
		[TestCase("1", null, "name", 100.0, "http://asd.sdf.com/asd.jpg")]
		[TestCase("1", "code", "", 100.0, "http://asd.sdf.com/asd.jpg")]
		[TestCase("1", "code", null, 100.0, "http://asd.sdf.com/asd.jpg")]
		[TestCase("1", "code", "name", 0, "http://asd.sdf.com/asd.jpg")]
		[TestCase("1", "code", "name", -100.0, "http://asd.sdf.com/asd.jpg")]
		[TestCase("1", "code", "name", 100.0, "asd.34")]
		[TestCase("1", "code", "name", 100.0, "D:\\asd\\sas.jpg")]
		public void UpdateAsync_NotValidInputData_ReturnsSuccess(string id, string code, string name, double price, string photo)
		{
			var product = new ProductInputModel
			{
				Code = code,
				Name = name,
				Price = price,
				Photo = photo
			};

			var productEntity = GetProductEntity(id, GetExistProduct());

			var repositoryMock = new Mock<IProductRepository>();
			repositoryMock.Setup(repository => repository.GetAsync(id)).ReturnsAsync(productEntity);
			repositoryMock.Setup(repository => repository.CheckIfProductCodeUniqueAsync(product.Code)).ReturnsAsync(true);

			var repositoryFactoryMock = new Mock<IProductRepositoryFactory>();
			repositoryFactoryMock.Setup(factory => factory.Get()).Returns(repositoryMock.Object);

			var handler = new ProductCommandsHandler(repositoryFactoryMock.Object);

			Assert.ThrowsAsync<ArgumentException>(async () => await handler.UpdateAsync(id, product));
		}

		[TestCase("23")]
		public void UpdateAsync_OnRepositoryException_ThrowSameException(string id)
		{
			var product = GetExistProduct();
            var productEntity = GetProductEntity(id, product);

            var exception = new Exception();

			var repositoryMock = new Mock<IProductRepository>();
            repositoryMock.Setup(repository => repository.CheckIfProductCodeUniqueAsync(product.Code)).ReturnsAsync(true);
            repositoryMock.Setup(repository => repository.GetAsync(id)).ReturnsAsync(productEntity);
            repositoryMock.Setup(repository => repository.UpdateAsync(productEntity)).Throws(exception);

			var repositoryFactoryMock =  new Mock<IProductRepositoryFactory>();
			repositoryFactoryMock.Setup(factory => factory.Get()).Returns(repositoryMock.Object);

			var handler = new ProductCommandsHandler(repositoryFactoryMock.Object);

			var result = Assert.ThrowsAsync(exception.GetType(), async () => await handler.UpdateAsync(id, product));			
			Assert.AreSame(exception, result);
		}


		#region Additional functions

		private Product GetProductEntity(string id, ProductInputModel product)
		{
			return new Product(id, product.Code, product.Name, product.Price, product.Photo);
		}

		private ProductInputModel GetNewProduct()
		{
			return new ProductInputModel
			{
				Code = "code",
				Name = "name",
				Price = 100.0
			};
		}
		
		private ProductInputModel GetExistProduct()
		{
			return new ProductInputModel
			{
				Code = "code",
				Name = "name",
				Price = 100.0
			};
        }

        private IUnitOfWork GetUnitOfWork()
        {
            var unit = new Mock<IUnitOfWork>();
            return unit.Object;
        }

        private IProductRepository GetRepository()
        {
            var repository = new Mock<IProductRepository>();
            return repository.Object;
        }

        private IProductRepositoryFactory GetProductRepositoryFactory()
		{
			var factoryMock = new Mock<IProductRepositoryFactory>();
			return factoryMock.Object;
		}

		#endregion
    }
}