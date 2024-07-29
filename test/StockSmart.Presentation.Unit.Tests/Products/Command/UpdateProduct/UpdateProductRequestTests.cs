using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NUnit.Framework;
using StockSmart.Application.Products.Command.UpdateProduct;
using StockSmart.Application.Unit.Tests.Helpers;

namespace StockSmart.Application.Unit.Tests.Products.Command.CreateProduct
{
    public class UpdateProductRequestTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void UpdateProductRequest_When_Model_Is_Valid_Should_Pass()
        {
            var model = this.BuildValidModel();
            var validationResult = ValidationHelper.ValidateModel(model);
            Assert.That(validationResult, Is.Empty);
        }

        [TestCase(null)]
        public void UpdateProductRequest_When_Weight_Is_Valid_Should_Pass(decimal? value)
        {
            var model = this.BuildValidModel(x => x.Weight = value);
            var validationResult = ValidationHelper.ValidateModel(model);
            Assert.That(validationResult, Is.Empty);
        }

        [TestCase(null)]
        public void UpdateProductRequest_When_Size_Is_Valid_Should_Pass(decimal? value)
        {
            var model = this.BuildValidModel(x => x.Size = value);

            var validationResult = ValidationHelper.ValidateModel(model);
            Assert.That(validationResult, Is.Empty);
        }

        [TestCase(null)]
        public void UpdateProductRequest_When_Discount_Is_Valid_Should_Pass(decimal? value)
        {
            var model = this.BuildValidModel(x => x.Discount = value);

            var validationResult = ValidationHelper.ValidateModel(model);
            Assert.That(validationResult, Is.Empty);
        }


        [TestCaseSource(nameof(InvalidProductId))]
        public void UpdateProductRequest_When_ProductId_Is_Invalid_Should_Fail(long value)
        {
            var model = this.BuildValidModel(x => x.ProductId = (int)value);
            var validationResult = ValidationHelper.ValidateModel(model);
            Assert.IsNotEmpty(validationResult);
            Assert.That(AssertValidationResult(validationResult, "ProductId"));
        }

        [TestCase("00000000000000000000000000000001")]
        [TestCase("")]
        public void UpdateProductRequest_When_ProductCode_Is_Invalid_Should_Fail(string value)
        {
            var model = this.BuildValidModel(x => x.ProductCode = value);
            var validationResult = ValidationHelper.ValidateModel(model);
            Assert.IsNotEmpty(validationResult);
            Assert.That(AssertValidationResult(validationResult, "ProductCode"));
        }

        [TestCaseSource(nameof(InvalidNames))]
        public void UpdateProductRequest_When_Name_Is_Invalid_Should_Fail(string value)
        {
            var model = this.BuildValidModel(x => x.Name = value);
            var validationResult = ValidationHelper.ValidateModel(model);
            Assert.IsNotEmpty(validationResult);
            Assert.That(AssertValidationResult(validationResult, "Name"));

        }

        [TestCase("")]
        public void UpdateProductRequest_When_StatusName_Is_Invalid_Should_Fail(string value)
        {
            var model = this.BuildValidModel(x => x.StatusName = value);
            var validationResult = ValidationHelper.ValidateModel(model);
            Assert.IsNotEmpty(validationResult);
            Assert.That(AssertValidationResult(validationResult, "StatusName"));
        }

        [TestCaseSource(nameof(InvalidStock))]
        public void UpdateProductRequest_When_Stock_Is_Invalid_Should_Fail(long value)
        {
            var model = this.BuildValidModel(x => x.Stock = (int)value);
            var validationResult = ValidationHelper.ValidateModel(model);
            Assert.IsNotEmpty(validationResult);
            Assert.That(AssertValidationResult(validationResult, "Stock"));
        }

        [TestCaseSource(nameof(InvalidDescription))]
        public void UpdateProductRequest_When_Description_Is_Invalid_Should_Fail(string value)
        {
            var model = this.BuildValidModel(x => x.Description = value);
            var validationResult = ValidationHelper.ValidateModel(model);
            Assert.IsNotEmpty(validationResult);
            Assert.That(AssertValidationResult(validationResult, "Description"));
        }

        [TestCaseSource(nameof(InvalidPrice))]
        public void UpdateProductRequest_When_Price_Is_Invalid_Should_Fail(decimal value)
        {
            var model = this.BuildValidModel(x => x.Price = value);
            var validationResult = ValidationHelper.ValidateModel(model);
            Assert.IsNotEmpty(validationResult);
            Assert.That(AssertValidationResult(validationResult, "Price"));
        }

        [TestCaseSource(nameof(InvalidDiscount))]
        public void UpdateProductRequest_When_Discount_Is_Invalid_Should_Fail(decimal value)
        {
            var model = this.BuildValidModel(x => x.Discount = value);
            var validationResult = ValidationHelper.ValidateModel(model);
            Assert.IsNotEmpty(validationResult);
            Assert.That(AssertValidationResult(validationResult, "Discount"));
        }

        [TestCaseSource(nameof(InvalidWeight))]
        public void UpdateProductRequest_When_Weight_Is_Invalid_Should_Fail(decimal value)
        {
            var model = this.BuildValidModel(x => x.Weight = value);
            var validationResult = ValidationHelper.ValidateModel(model);
            Assert.IsNotEmpty(validationResult);
            Assert.That(AssertValidationResult(validationResult, "Weight"));
        }

        [TestCaseSource(nameof(InvalidSize))]
        public void UpdateProductRequest_When_Size_Is_Invalid_Should_Fail(decimal value)
        {
            var model = this.BuildValidModel(x => x.Size = value);
            var validationResult = ValidationHelper.ValidateModel(model);
            Assert.IsNotEmpty(validationResult);
            Assert.That(AssertValidationResult(validationResult, "Size"));
        }

        private UpdateProductRequest BuildValidModel(Action<UpdateProductRequest> value = null)
        {
            var UpdateProductRequest = new UpdateProductRequest
            {
                ProductId = int.MaxValue,
                ProductCode = new string('A', 30),
                Name = new string('A', 500),
                StatusName = "Active",
                Stock = int.MaxValue,
                Description = new string('A', 5000),
                Price = decimal.MaxValue,
                Discount = 100,
                Weight = decimal.MaxValue,
                Size = decimal.MaxValue
            };

            value?.Invoke(UpdateProductRequest);

            return UpdateProductRequest;
        }

        private static bool AssertValidationResult(IList<ValidationResult> validationResult, string message) => validationResult
                                    .Any(result => result.ErrorMessage != null && result.ErrorMessage.Contains(message, StringComparison.OrdinalIgnoreCase));

        private static long[] InvalidProductId =
        {
            int.MaxValue + (long)1,
            -1
        };

        private static object[] InvalidNames =
        {
            new string('A', 501),
            string.Empty
        };

        private static long[] InvalidStock =
        {
            int.MaxValue + (long)1,
            -1
        };

        private static object[] InvalidDescription =
        {
            new string('A', 5001),
            string.Empty
        };

        private static decimal[] InvalidPrice =
        {
            -1
        };

        private static decimal[] InvalidWeight =
        {
            -1
        };

        private static decimal[] InvalidSize =
        {
            -1
        };

        private static decimal[] InvalidDiscount =
        {
            101,
            -1
        };
    }
}
