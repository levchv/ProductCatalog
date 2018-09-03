using System;
using System.Linq;
using NUnit.Framework;
using ProductCatalog.Domain.Core.DataModels;
using ProductCatalog.Domain.Products.Entities;

namespace ProductCatalog.Domain.Tests.Products.Entities
{
    public class ProductTests
    {
		[Test]
		public void ClassImplementsIEntityInterface()
		{
			var type = typeof(Product);
			var @interface = typeof(IEntity);
			Assert.IsTrue(type.GetInterfaces().Contains(@interface));
		}
		
		[TestCase("code", "name", 100.0, "http://asd.sdf.com/asd.jpg")]
		[TestCase("code2", "name2", 12.12, "")]
		[TestCase("code3", "name3", 100.0, null)]
		public void ClassCouldBeConstructNewWithValidData(string code, string name, double price, string photo)
		{
			Assert.DoesNotThrow(() => new Product(code, name, price, photo));
		}
		
		[TestCase("1", "code", "name", 100.0, "http://asd.sdf.com/asd.jpg")]
		[TestCase("2", "code2", "name2", 12.12, "")]
		[TestCase("3", "code3", "name3", 100.0, null)]
		public void ClassCouldBeConstructExistingWithValidData(string id, string code, string name, double price, string photo)
		{
			Assert.DoesNotThrow(() => new Product(id, code, name, price, photo));
		}
		
		
		[TestCase("", "name", 100.0, "http://asd.sdf.com/asd.jpg")]
		[TestCase(null, "name", 100.0, "http://asd.sdf.com/asd.jpg")]
		[TestCase("code", "", 100.0, "http://asd.sdf.com/asd.jpg")]
		[TestCase("code", null, 100.0, "http://asd.sdf.com/asd.jpg")]
		[TestCase("code", "name", 0, "http://asd.sdf.com/asd.jpg")]
		[TestCase("code", "name", -100.0, "http://asd.sdf.com/asd.jpg")]
		public void ClassCouldNotBeConstructNewWithNotValidData(string code, string name, double price, string photo)
		{
			Assert.Throws<ArgumentException>(() => new Product(code, name, price, photo));
		}
		
		[TestCase("1", "", "name", 100.0, "http://asd.sdf.com/asd.jpg")]
		[TestCase("1", null, "name", 100.0, "http://asd.sdf.com/asd.jpg")]
		[TestCase("1", "code", "", 100.0, "http://asd.sdf.com/asd.jpg")]
		[TestCase("1", "code", null, 100.0, "http://asd.sdf.com/asd.jpg")]
		[TestCase("1", "code", "name", 0, "http://asd.sdf.com/asd.jpg")]
		[TestCase("1", "code", "name", -100.0, "http://asd.sdf.com/asd.jpg")]
		[TestCase("", "code", "name", 100.0, "http://asd.sdf.com/asd.jpg")]
		[TestCase(null, "code", "name", 100.0, "http://asd.sdf.com/asd.jpg")]
		public void ClassCouldNotBeConstructExistingWithNotValidData(string id, string code, string name, double price, string photo)
		{
			Assert.Throws<ArgumentException>(() => new Product(id, code, name, price, photo));
		}
		
		[TestCase("1", ExpectedResult="1")]
		public string GetId_ReturnsExpetedResult(string value)
		{
			var product = new Product(value, "code", "name",  100.0, "http://asd.sdf.com/asd.jpg");
			return product.GetId();
		}
		
		[TestCase("ASDC", ExpectedResult="ASDC")]
		public string GetCode_ReturnsExpetedResult(string value)
		{
			var product = new Product("id", value, "name", 100.0, "http://asd.sdf.com/asd.jpg");
			return product.GetCode();
		}
		
		[TestCase("Prod", ExpectedResult="Prod")]
		public string GetName_ReturnsExpetedResult(string value)
		{
			var product = new Product("id", "code", value, 100.0, "http://asd.sdf.com/asd.jpg");
			return product.GetName();
		}
		
		[TestCase(12.23, ExpectedResult=12.23)]
		public double GetProce_ReturnsExpetedResult(double value)
		{
			var product = new Product("id", "code", "name", value, "http://asd.sdf.com/asd.jpg");
			return product.GetPrice();
		}
		
		[TestCase("http://asd.sdf.com/asdasd.jpg", ExpectedResult="http://asd.sdf.com/asdasd.jpg")]
		public string GetPhoto_ReturnsExpetedResult(string value)
		{
			var product = new Product("id", "name", "code", 100.0, value);
			return product.GetPhoto();
		}
				
		[TestCase("ASDC")]
		public void SetCode_ValidValue_DoesNotThrowException(string value)
		{
			var product = new Product("id", "code", "name", 100.0, "http://asd.sdf.com/asd.jpg");
			Assert.DoesNotThrow(() => product.SetCode(value));
		}

		[TestCase("ASDC", ExpectedResult="ASDC")]
		public string GetCode_AfterSet_ReturnsExpectedResult(string value)
		{
			var product = new Product("id", "code","name",  100.0, "http://asd.sdf.com/asd.jpg");
			product.SetCode(value);
			return product.GetCode();
		}
			
		[TestCase("")]
		[TestCase(null)]
		public void SetCode_NotValidValue_ThrowsArgumentException(string value)
		{
			var product = new Product("id", "code", "name", 100.0, "http://asd.sdf.com/asd.jpg");
			Assert.Throws<ArgumentException>(() => product.SetCode(value));
		}
				
		[TestCase("Prod")]
		public void SetName_ValidValue_DoesNotThrowException(string value)
		{
			var product = new Product("id", "code", "name", 100.0, "http://asd.sdf.com/asd.jpg");
			Assert.DoesNotThrow(() => product.SetName(value));
		}

		[TestCase("Prod", ExpectedResult="Prod")]
		public string GetName_AfterSet_ReturnsExpectedResult(string value)
		{
			var product = new Product("id", "code", "name", 100.0, "http://asd.sdf.com/asd.jpg");
			product.SetName(value);
			return product.GetName();
		}
			
		[TestCase("")]
		[TestCase(null)]
		public void SetName_NotValidValue_ThrowsArgumentException(string value)
		{
			var product = new Product("id", "code", "name", 100.0, "http://asd.sdf.com/asd.jpg");
			Assert.Throws<ArgumentException>(() => product.SetName(value));
		}
				
				
		[TestCase(123.7)]
		public void SetPrice_ValidValue_DoesNotThrowException(double value)
		{
			var product = new Product("id", "code", "name", 100.0, "http://asd.sdf.com/asd.jpg");
			Assert.DoesNotThrow(() => product.SetPrice(value));
		}

		[TestCase(123.7, ExpectedResult=123.7)]
		public double GetPrice_AfterSet_ReturnsExpectedResult(double value)
		{
			var product = new Product("id", "code", "name", 100.0, "http://asd.sdf.com/asd.jpg");
			product.SetPrice(value);
			return product.GetPrice();
		}
			
		[TestCase(-123.43)]
		[TestCase(0)]
		public void SetPrice_NotValidValue_ThrowsArgumentException(double value)
		{
			var product = new Product("id", "code", "name", 100.0, "http://asd.sdf.com/asd.jpg");
			Assert.Throws<ArgumentException>(() => product.SetPrice(value));
		}
				
		[TestCase("http://asd.asd.com/asd2.jpg")]
		public void SetPhoto_ValidValue_DoesNotThrowException(string value)
		{
			var product = new Product("id", "code", "name", 100.0, "http://asd.sdf.com/asd.jpg");
			Assert.DoesNotThrow(() => product.SetPhoto(value));
		}

		[TestCase("http://asd.asd.com/asd2.jpg", ExpectedResult="http://asd.asd.com/asd2.jpg")]
		public string GetPhoto_AfterSet_ReturnsExpectedResult(string value)
		{
			var product = new Product("id", "code", "name", 100.0, "http://asd.sdf.com/asd.jpg");
			product.SetPhoto(value);
			return product.GetPhoto();
		}
			
		[TestCase("d:\\tmp\\asd.jpg")]
		[TestCase("asdsad.jpg")]
		public void SetPhoto_NotValidValue_ThrowsArgumentException(string value)
		{
			var product = new Product("id", "code", "name", 100.0, "http://asd.sdf.com/asd.jpg");
			Assert.Throws<ArgumentException>(() => product.SetPhoto(value));
		}
    }
}