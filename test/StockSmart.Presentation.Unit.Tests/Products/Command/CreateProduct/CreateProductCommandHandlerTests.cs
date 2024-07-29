using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using StockSmart.Application.Dto;
using StockSmart.Application.Products.Command.CreateProduct;
using StockSmart.Application.Products.Mappers;
using StockSmart.Application.Products.Mappers.Abstract;
using StockSmart.Application.Services;
using StockSmart.Domain.Common.Abstract;
using StockSmart.Domain.Entities;
using StockSmart.Domain.Exceptions;

namespace StockSmart.Application.Unit.Tests.Products.Command.CreateProduct
{
    [TestFixture]
    public class CreateProductCommandHandlerTests
    {
        private Mock<IProductRepository> _mockProductRepository;
        private IProductMapper _productMapper;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IDiscountService> _mockDiscountService;
        private Mock<IStatusRepository> _mockStatusRepository;
        private Mock<ILogger<CreateProductCommandHandler>> _mockLogger;
        private Mock<ILoggerFactory> _mockLoggerFactory;

        private CreateProductCommandHandler _handler;

        private IEnumerable<Status> _statuses;

        [SetUp]
        public void Setup()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _productMapper = new ProductMapper();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockDiscountService = new Mock<IDiscountService>();
            _mockStatusRepository = new Mock<IStatusRepository>();
            _mockLogger = new Mock<ILogger<CreateProductCommandHandler>>();
            _mockLoggerFactory = new Mock<ILoggerFactory>();

            _statuses = new List<Status>
            {
                Status.Create(1, "Active"),
                Status.Create(2, "Inactive")
            };

            _mockLoggerFactory
                .Setup(x => x.CreateLogger(It.IsAny<string>()))
                .Returns(() => _mockLogger.Object);

            _mockStatusRepository
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(_statuses);

            _handler = new CreateProductCommandHandler(
                _mockProductRepository.Object,
                _productMapper,
                _mockUnitOfWork.Object,
                _mockDiscountService.Object,
                _mockStatusRepository.Object,
                _mockLoggerFactory.Object);
        }

        [Test]
        public async Task Handle_ShouldCreateProducts_WhenAllDependenciesAreValid()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Products = new List<CreateProductRequest>
                {
                    BuildValidProductModel()
                }
            };

            var discounts = new List<DiscountDto>
            {
                new DiscountDto { ProductCode = "P001", Value = 10 }
            };

            _mockDiscountService
                .Setup(d => d.GetDiscountByProducts(It.IsAny<List<string>>()))
                .ReturnsAsync(discounts);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.AreEqual(1, result.Count());
            _mockProductRepository.Verify(r => r.AddRange(It.IsAny<IEnumerable<Product>>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.Complete(), Times.Once);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenStatusIsInvalid()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Products = new List<CreateProductRequest>
                {
                    BuildValidProductModel(x => x.StatusName = "InvalidStatus")
                }
            };

            // Act & Assert
            Assert.ThrowsAsync<ProductStatusInvalidException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public async Task Handle_ShouldApplyDiscount_WhenDiscountIsAvailable()
        {
            // Arrange
            var price = 100;
            var discount = 10;
            var command = new CreateProductCommand
            {
                Products = new List<CreateProductRequest>
                {
                    BuildValidProductModel(x => x.Price = price)
                }
            };

            var discounts = new List<DiscountDto>
            {
                new DiscountDto { ProductCode = "P001", Value = discount }
            };

            _mockDiscountService
                .Setup(d => d.GetDiscountByProducts(It.IsAny<List<string>>()))
                .ReturnsAsync(discounts);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(10, result.First().Discount);
            Assert.AreEqual(CalculateFinalPrice(price, discount), result.First().FinalPrice);
        }

        [Test]
        public async Task Handle_ShouldNotApplyDiscount_WhenNoDiscountIsAvailable()
        {
            // Arrange
            var price = 100;
            var command = new CreateProductCommand
            {
                Products = new List<CreateProductRequest>
                {
                    BuildValidProductModel(x => x.Price = price)
                }
            };

            var discounts = new List<DiscountDto>();

            _mockDiscountService
                .Setup(d => d.GetDiscountByProducts(It.IsAny<List<string>>()))
                .ReturnsAsync(discounts);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(0, result.First().Discount);
            Assert.AreEqual(price, result.First().FinalPrice);
        }

        [Test]
        public async Task Handle_ShouldCreateMultipleProducts_WhenAllDependenciesAreValid()
        {
            // Arrange
            var discount = 10;
            var command = new CreateProductCommand
            {
                Products = new List<CreateProductRequest>
                {
                    BuildValidProductModel(),
                    BuildValidProductModel(x =>
                    {
                        x.ProductCode = "P002";
                        x.StatusName = "Inactive";
                    })
                }
            };

            var discounts = new List<DiscountDto>
            {
                new DiscountDto { ProductCode = "P001", Value = discount }
            };

            _mockDiscountService
                .Setup(d => d.GetDiscountByProducts(It.IsAny<List<string>>()))
                .ReturnsAsync(discounts);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(discount, result.First(x => x.ProductCode == "P001").Discount);
            Assert.AreEqual(0, result.First(x => x.ProductCode == "P002").Discount);
        }

        [Test]
        public async Task Handle_ShouldLogInformation_WhenHandlingCommand()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Products = new List<CreateProductRequest>
                {
                    BuildValidProductModel(),
                }
            };

            var discounts = new List<DiscountDto>
            {
                new DiscountDto { ProductCode = "P001", Value = 10 }
            };

            _mockDiscountService
                .Setup(d => d.GetDiscountByProducts(It.IsAny<List<string>>()))
                .ReturnsAsync(discounts);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockLogger.Verify(
                logger => logger.Log<It.IsAnyType>(
                    It.Is<LogLevel>(level => level == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.AtLeastOnce);
        }

        [Test]
        public async Task Handle_ShouldThrowException_WhenDiscountIsOutOfRange()
        {
            // Arrange
            var discount = -10;
            var command = new CreateProductCommand
            {
                Products = new List<CreateProductRequest>
                {
                    BuildValidProductModel()
                }
            };

            var discounts = new List<DiscountDto>
            {
                new DiscountDto { ProductCode = "P001", Value = discount } // Invalid discount
            };

            _mockDiscountService
                .Setup(d => d.GetDiscountByProducts(It.IsAny<List<string>>()))
                .ReturnsAsync(discounts);

            // Act & Assert
            Assert.ThrowsAsync<DiscountInvalidException>(() => _handler.Handle(command, CancellationToken.None));
        }



        private decimal CalculateFinalPrice(decimal price, decimal discount) => price * (100 - discount) / 100;

        private CreateProductRequest BuildValidProductModel(Action<CreateProductRequest> value = null)
        {
            var createProductRequest = new CreateProductRequest
            {
                ProductCode = "P001",
                Name = "Product 1",
                StatusName = "Active",
                Stock = 150,
                Description = "The description of the product 1",
                Price = 100,
                Weight = 30,
                Size = 50
            };

            value?.Invoke(createProductRequest);

            return createProductRequest;
        }
    }
}
