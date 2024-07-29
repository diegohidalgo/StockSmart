using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NUnit.Framework;
using StockSmart.Application.Products.Command.CreateProduct;
using StockSmart.Application.Unit.Tests.Helpers;

namespace StockSmart.Application.Unit.Tests.Products.Command.CreateProduct
{
    public class CreateProductRequestTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CreateProductRequest_When_Model_Is_Valid_Should_Pass()
        {
            var model = this.BuildValidModel();
            var validationResult = ValidationHelper.ValidateModel(model);
            Assert.That(validationResult, Is.Empty);
        }

        [TestCase(null)]
        public void CreateProductRequest_When_Weight_Is_Valid_Should_Pass(decimal? value)
        {
            var model = this.BuildValidModel(x => x.Weight = value);
            var validationResult = ValidationHelper.ValidateModel(model);
            Assert.That(validationResult, Is.Empty);
        }

        [TestCase(null)]
        public void CreateProductRequest_When_Size_Is_Valid_Should_Pass(decimal? value)
        {
            var model = this.BuildValidModel(x => x.Size = value);

            var validationResult = ValidationHelper.ValidateModel(model);
            Assert.That(validationResult, Is.Empty);
        }


        [TestCase("00000000000000000000000000000001")]
        [TestCase("")]
        public void CreateProductRequest_When_ProductCode_Is_Invalid_Should_Fail(string value)
        {
            var model = this.BuildValidModel(x => x.ProductCode = value);
            var validationResult = ValidationHelper.ValidateModel(model);
            Assert.IsNotEmpty(validationResult);
            Assert.That(AssertValidationResult(validationResult, "ProductCode"));
        }

        [TestCaseSource(nameof(InvalidNames))]
        public void CreateProductRequest_When_Name_Is_Invalid_Should_Fail(string value)
        {
            var model = this.BuildValidModel(x => x.Name = value);
            var validationResult = ValidationHelper.ValidateModel(model);
            Assert.IsNotEmpty(validationResult);
            Assert.That(AssertValidationResult(validationResult, "Name"));

        }

        [TestCase("")]
        public void CreateProductRequest_When_StatusName_Is_Invalid_Should_Fail(string value)
        {
            var model = this.BuildValidModel(x => x.StatusName = value);
            var validationResult = ValidationHelper.ValidateModel(model);
            Assert.IsNotEmpty(validationResult);
            Assert.That(AssertValidationResult(validationResult, "StatusName"));
        }

        [TestCaseSource(nameof(InvalidStock))]

        public void CreateProductRequest_When_Stock_Is_Invalid_Should_Fail(long value)
        {
            var model = this.BuildValidModel(x => x.Stock = (int)value);
            var validationResult = ValidationHelper.ValidateModel(model);
            Assert.IsNotEmpty(validationResult);
            Assert.That(AssertValidationResult(validationResult, "Stock"));
        }

        [TestCaseSource(nameof(InvalidDescription))]
        public void CreateProductRequest_When_Description_Is_Invalid_Should_Fail(string value)
        {
            var model = this.BuildValidModel(x => x.Description = value);
            var validationResult = ValidationHelper.ValidateModel(model);
            Assert.IsNotEmpty(validationResult);
            Assert.That(AssertValidationResult(validationResult, "Description"));
        }

        [TestCaseSource(nameof(InvalidPrice))]

        public void CreateProductRequest_When_Price_Is_Invalid_Should_Fail(decimal value)
        {
            var model = this.BuildValidModel(x => x.Price = value);
            var validationResult = ValidationHelper.ValidateModel(model);
            Assert.IsNotEmpty(validationResult);
            Assert.That(AssertValidationResult(validationResult, "Price"));
        }

        [TestCaseSource(nameof(InvalidWeight))]

        public void CreateProductRequest_When_Weight_Is_Invalid_Should_Fail(decimal value)
        {
            var model = this.BuildValidModel(x => x.Weight = value);
            var validationResult = ValidationHelper.ValidateModel(model);
            Assert.IsNotEmpty(validationResult);
            Assert.That(AssertValidationResult(validationResult, "Weight"));
        }

        [TestCaseSource(nameof(InvalidSize))]

        public void CreateProductRequest_When_Size_Is_Invalid_Should_Fail(decimal value)
        {
            var model = this.BuildValidModel(x => x.Size = value);
            var validationResult = ValidationHelper.ValidateModel(model);
            Assert.IsNotEmpty(validationResult);
            Assert.That(AssertValidationResult(validationResult, "Size"));
        }

        private CreateProductRequest BuildValidModel(Action<CreateProductRequest> value = null)
        {
            var createProductRequest = new CreateProductRequest
            {
                ProductCode = new string('A', 30),
                Name = new string('A', 500),
                StatusName = "Active",
                Stock = int.MaxValue,
                Description = new string('A', 5000),
                Price = decimal.MaxValue,
                Weight = decimal.MaxValue,
                Size = decimal.MaxValue
            };

            value?.Invoke(createProductRequest);

            return createProductRequest;
        }

        private static bool AssertValidationResult(IList<ValidationResult> validationResult, string message) => validationResult
                                    .Any(result => result.ErrorMessage != null && result.ErrorMessage.Contains(message, StringComparison.OrdinalIgnoreCase));

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
    }
}
