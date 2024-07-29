using System;
using System.Collections.Generic;
using System.Data;
using StockSmart.Domain.Exceptions;

namespace StockSmart.Domain.Entities
{
    public class Product : IEquatable<Product>
    {
        public Product()
        {

        }
        private Product(int productId, string productCode, string name, Status status, int stock, string description, decimal price, decimal discount, decimal? weight, decimal? size)
        {
            this.ProductId = productId;
            this.ProductCode = productCode;
            this.Name = name;
            this.Status = status;
            this.Stock = stock;
            this.Description = description;
            this.Price = price;
            SetDiscount(discount);
            this.Weight = weight;
            this.Size = size;
        }

        public int ProductId { get; private set; }
        public string ProductCode { get; private set; }
        public string Name { get; private set; }
        public int StatusId { get; private set; }
        public Status Status { get; private set; }
        public int Stock { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public decimal Discount { get; private set; }
        public decimal FinalPrice => this.Price * (100 - this.Discount) / 100;
        public decimal? Weight { get; private set; }
        public decimal? Size { get; private set; }


        public static Product Create(int productId, string productCode, string name, Status status, int stock, string description, decimal price, decimal discount, decimal? weight, decimal? size) =>
            new Product(productId, productCode, name, status, stock, description, price, discount, weight, size);

        public Product Update(string productCode = null, string name = null, int? statusId = null, Status status = null, int? stock = null, string description = null,
            decimal? price = null, decimal? discount = null, decimal? weight = null, decimal? size = null)
        {
            ProductCode = productCode ?? ProductCode;
            Name = name ?? Name;
            StatusId = statusId ?? StatusId;
            Status = status ?? Status;
            Stock = stock ?? Stock;
            Description = description ?? Description;
            Price = price ?? Price;
            SetDiscount(discount ?? Discount);
            Weight = weight ?? Weight;
            Size = size ?? Size;
            return this;
        }

        public void UpdateStatus(int statusId)
        {
            StatusId = statusId;
            Status = null;
        }

        public void UpdateStatus(Status status)
        {
            Status = status ?? throw new ProductStatusInvalidException("Invalid status", status);
            StatusId = status.StatusId;
        }
        public void SetDiscount(decimal? discount)
        {
            if (discount < 0 || discount > 100)
            {
                throw new DiscountInvalidException("Invalid discount, out of range", discount);
            }
            Discount = discount ?? 0;
        }

        #region Equatable
        public override bool Equals(object obj) => this.Equals(obj as Product);
        public bool Equals(Product other) => !(other is null) && this.ProductId == other.ProductId;
        public override int GetHashCode() => HashCode.Combine(this.ProductId);

        public static bool operator ==(Product left, Product right) => EqualityComparer<Product>.Default.Equals(left, right);
        public static bool operator !=(Product left, Product right) => !(left == right);
        #endregion
    }
}
