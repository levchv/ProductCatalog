using System;
using System.Linq;
using NUnit.Framework;
using ProductCatalog.Domain.Core.DataModels;
using ProductCatalog.Domain.Products.InputModels;

namespace ProductCatalog.Domain.Tests.Products.InputModels
{
    public class ProductInputModelTests
    {
		[Test]
		public void ClassImplementsIEntityInterface()
		{
			var type = typeof(ProductInputModel);
			var @interface = typeof(IInputModel);
			Assert.IsTrue(type.GetInterfaces().Contains(@interface));
		}

		[Test]
		public void ClassHasParameterlessConstuctor()
		{
			var type = typeof(ProductInputModel);
			var constructor = type.GetConstructor(Type.EmptyTypes);
			Assert.IsNotNull(constructor);
		}
        
		[Test]
		public void ClassCouldBeConstruct()
		{
			Assert.DoesNotThrow(() => new ProductInputModel());
		}
		[Test]
		public void ClassHasStringGetSetCodeProperty()
		{
			var type = typeof(ProductInputModel);
			var property = type.GetProperty("Code");

			Assert.IsNotNull(property);			
			Assert.IsNotNull(property.GetMethod);
			Assert.IsNotNull(property.SetMethod);
			Assert.AreEqual(typeof(string), property.PropertyType);
		}
        
		[Test]
		public void ClassHasStringGetSetNameProperty()
		{
			var type = typeof(ProductInputModel);
			var property = type.GetProperty("Name");

			Assert.IsNotNull(property);			
			Assert.IsNotNull(property.GetMethod);
			Assert.IsNotNull(property.SetMethod);
			Assert.AreEqual(typeof(string), property.PropertyType);
		}
        
		[Test]
		public void ClassHasStringGetSetPriceProperty()
		{
			var type = typeof(ProductInputModel);
			var property = type.GetProperty("Price");

			Assert.IsNotNull(property);			
			Assert.IsNotNull(property.GetMethod);
			Assert.IsNotNull(property.SetMethod);
			Assert.AreEqual(typeof(double), property.PropertyType);
		}
        
		[Test]
		public void ClassHasStringGetSetPhotoProperty()
		{
			var type = typeof(ProductInputModel);
			var property = type.GetProperty("Photo");

			Assert.IsNotNull(property);			
			Assert.IsNotNull(property.GetMethod);
			Assert.IsNotNull(property.SetMethod);
			Assert.AreEqual(typeof(string), property.PropertyType);
		}
    }
}