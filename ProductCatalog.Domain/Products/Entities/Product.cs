using System;
using ProductCatalog.Domain.Core.DataModels;

namespace ProductCatalog.Domain.Products.Entities
{
    public sealed class Product: IEntity
    {
		private string code;
		private string name;
		private double price;
		private string photo;
		private string id;

		public Product(string code, string name, double price, string photo)
		{
			SetCode(code);
			SetName(name);
			SetPrice(price);
			SetPhoto(photo);
		}
		public Product(string id, string code, string name, double price, string photo)
			: this(code, name, price, photo)
		{
			SetId(id);
		}

		private Product() { }

		#region Update functions

		private void SetId(string id)
		{
			if (string.IsNullOrEmpty(id))
				throw new ArgumentException("Value is empty", nameof(id));

			this.id = id;
		}

		public void SetCode(string code)
		{
			if (string.IsNullOrEmpty(code))
				throw new ArgumentException("Value is empty", nameof(code));

			this.code = code;
		}

		public void SetName(string name)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentException("Value is empty", nameof(name));

			this.name = name;
		}

		public void SetPrice(double price)
		{
			if (price < 0 || price < Double.Epsilon)
				throw new ArgumentException("Value is less or equals zero", nameof(price));

			this.price = price;
		}

		public void SetPhoto(string photo)
		{
			if (!string.IsNullOrEmpty(photo) && !Uri.IsWellFormedUriString(photo, UriKind.Absolute))
				throw new ArgumentException("Value is not well formed absolute uri", nameof(photo));

			this.photo = photo;
		}

		#endregion

		#region Get functions

		public string GetId()
		{
			return this.id;
		}

		public string GetCode()
		{
			return this.code;
		}

		public string GetName()
		{
			return this.name;
		}

		public double GetPrice()
		{
			return this.price;
		}

		public string GetPhoto()
		{
			return this.photo;
		}

		#endregion

	}
}