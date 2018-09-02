using System;
using System.Linq;
using NUnit.Framework;
using ProductCatalog.Domain.Core.DataModels;
using ProductCatalog.Domain.Products.Ports.Driving.Queries.ViewModels;

namespace ProductCatalog.Domain.Tests.Products.ViewModels
{
    public class ProductViewModelTests
    {
		[Test]
		public void ClassImplementsIEntityInterface()
		{
			var type = typeof(ProductViewModel);
			var @interface = typeof(IViewModel);
			Assert.IsTrue(type.GetInterfaces().Contains(@interface));
		}
		
        
		[Test]
		public void ClassCouldBeConstruct()
		{
			Assert.DoesNotThrow(() => new ProductViewModel("1", "CODE", "Name", 123.21, "http://asd.asd/asd.jpg", DateTime.Now));
		}
        
		[Test]
		public void ClassHasStringGetIdProperty()
		{
			var type = typeof(ProductViewModel);
			var property = type.GetProperty("Id");

			Assert.IsNotNull(property);			
			Assert.IsNotNull(property.GetMethod);
			Assert.AreEqual(typeof(string), property.PropertyType);
		}
        
		[Test]
		public void ClassHasStringGetCodeProperty()
		{
			var type = typeof(ProductViewModel);
			var property = type.GetProperty("Code");

			Assert.IsNotNull(property);			
			Assert.IsNotNull(property.GetMethod);
			Assert.AreEqual(typeof(string), property.PropertyType);
		}
        
		[Test]
		public void ClassHasStringGetNameProperty()
		{
			var type = typeof(ProductViewModel);
			var property = type.GetProperty("Name");

			Assert.IsNotNull(property);			
			Assert.IsNotNull(property.GetMethod);
			Assert.AreEqual(typeof(string), property.PropertyType);
		}
        
		[Test]
		public void ClassHasStringGetPriceProperty()
		{
			var type = typeof(ProductViewModel);
			var property = type.GetProperty("Price");

			Assert.IsNotNull(property);			
			Assert.IsNotNull(property.GetMethod);
			Assert.AreEqual(typeof(double), property.PropertyType);
		}
        
		[Test]
		public void ClassHasStringGetPhotoProperty()
		{
			var type = typeof(ProductViewModel);
			var property = type.GetProperty("Photo");

			Assert.IsNotNull(property);			
			Assert.IsNotNull(property.GetMethod);
			Assert.AreEqual(typeof(string), property.PropertyType);
		}
        
		[Test]
		public void ClassHasStringGetLastUpdatedProperty()
		{
			var type = typeof(ProductViewModel);
			var property = type.GetProperty("LastUpdated");

			Assert.IsNotNull(property);			
			Assert.IsNotNull(property.GetMethod);
			Assert.AreEqual(typeof(DateTime), property.PropertyType);
		}
    }
}