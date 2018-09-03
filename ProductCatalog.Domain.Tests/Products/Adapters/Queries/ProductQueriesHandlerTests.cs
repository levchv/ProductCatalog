using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using ProductCatalog.Domain.Products.Adapters.Queries;
using ProductCatalog.Domain.Products.Ports.Driven;
using ProductCatalog.Domain.Products.ViewModels;

namespace ProductCatalog.Domain.Tests.Products.Adapters.Queries
{
    public class ProductQueriesHandlerTests
    {
         
		[Test]
		public void ClassCouldBeConstruct()
		{
			var repository = GetProductReadOnlyRepository();
			Assert.DoesNotThrow(() => new ProductQueriesHandler(repository));
		}

		[Test]
		public void ConstructorRequiresProductQueriesHandler()
		{	
			Assert.Throws<ArgumentNullException>(() => new ProductQueriesHandler(null));
		}

		[Test]
		public async Task GetAllAsync_ReturnsAllProducts()
		{
			var products = GetProducts();

			var repositoryMock = new Mock<IProductReadOnlyRepository>();
			repositoryMock.Setup(repository => repository.GetAllAsync()).ReturnsAsync(products);

			var handler = new ProductQueriesHandler(repositoryMock.Object);
			
			var result = await handler.GetAllAsync();
			Assert.AreEqual(products, result);
		}

		[Test]
		public void GetAllAsync_OnInnerException_ThrowsSameException()
		{
			var exception = new Exception();

			var repositoryMock = new Mock<IProductReadOnlyRepository>();
			repositoryMock.Setup(repository => repository.GetAllAsync()).ThrowsAsync(exception);

			var handler = new ProductQueriesHandler(repositoryMock.Object);

			var result = Assert.ThrowsAsync(exception.GetType(), async () => await handler.GetAllAsync());
			Assert.AreEqual(exception, result);
		}

		[TestCase("some text")]
		public async Task SearchAsync_ValidParams_ReturnsProducts(string search)
		{
			var products = GetProducts();

			var repositoryMock = new Mock<IProductReadOnlyRepository>();
			repositoryMock.Setup(repository => repository.SearchAsync(search)).ReturnsAsync(products);

			var handler = new ProductQueriesHandler(repositoryMock.Object);
			
			var result = await handler.SearchAsync(search);
			Assert.AreEqual(products, result);
		}
		
		[TestCase("")]
		[TestCase(null)]
		public async Task SearchAsync_NotValidParams_ReturnsEmptyArray(string search)
		{
			var repository = GetProductReadOnlyRepository();

			var handler = new ProductQueriesHandler(repository);
			
			var result = await handler.SearchAsync(search);
			Assert.AreEqual(0, result.Length);
		}

        [TestCase("some text")]
        public void SearchAsync_OnInnerException_ThrowsSameException(string search)
		{
			var exception = new Exception();

			var repositoryMock = new Mock<IProductReadOnlyRepository>();
			repositoryMock.Setup(repository => repository.SearchAsync(search)).ThrowsAsync(exception);

			var handler = new ProductQueriesHandler(repositoryMock.Object);

			var result = Assert.ThrowsAsync(exception.GetType(), async () => await handler.SearchAsync(search));
			Assert.AreEqual(exception, result);
		}

		[TestCase("23", true)]
		[TestCase("23", false)]
		public async Task GetAsync_ValidId_ReturnsRepositoryResult(string id, bool repositoryReturnsProduct)
		{
			var product = repositoryReturnsProduct ? GetProduct(id) : null;

			var repositoryMock = new Mock<IProductReadOnlyRepository>();
			repositoryMock.Setup(repository => repository.GetAsync(id)).ReturnsAsync(product);

			var handler = new ProductQueriesHandler(repositoryMock.Object);
			
			var result = await handler.GetAsync(id);
			Assert.AreEqual(product, result);
		}

		[TestCase(null)]
		[TestCase("")]
		public async Task GetAsync_NotValidId_ReturnsProduct(string id)
		{
			var repository = GetProductReadOnlyRepository();

			var handler = new ProductQueriesHandler(repository);
			
			var result = await handler.GetAsync(id);
			Assert.IsNull(result);
		}
		
		[TestCase("23")]
		public void GetAsync_OnInnerException_ThrowsSameException(string id)
		{
			var exception = new Exception();

			var repositoryMock = new Mock<IProductReadOnlyRepository>();
			repositoryMock.Setup(repository => repository.GetAsync(id)).ThrowsAsync(exception);

			var handler = new ProductQueriesHandler(repositoryMock.Object);

			var result = Assert.ThrowsAsync(exception.GetType(), async () => await handler.GetAsync(id));
			Assert.AreEqual(exception, result);
		}

		#region Additional functions

		private IProductReadOnlyRepository GetProductReadOnlyRepository()
		{
			var repository = new Mock<IProductReadOnlyRepository>();
			return repository.Object;
		}

		private ProductViewModel GetProduct(string id)
		{
			return new ProductViewModel(id, "code", "name", 100.0, "photo", DateTime.Now);
		}

		private ProductViewModel[] GetProducts()
		{
			return new ProductViewModel[]
			{
				GetProduct("1"),
				GetProduct("23")
			};
		}

		#endregion
    }
}