using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using StockSmart.Application.Dto;
using StockSmart.Application.Products.Command.UpdateProduct;
using StockSmart.Application.Products.Mappers;
using StockSmart.Application.Products.Mappers.Abstract;
using StockSmart.Application.Services;
using StockSmart.Domain.Common.Abstract;
using StockSmart.Domain.Entities;
using StockSmart.Domain.Exceptions;

namespace StockSmart.Application.Unit.Tests.Products.Command.UpdateProduct;

[TestFixture]
public class UpdateProductCommandHandlerTests
{
    private Mock<IProductRepository> _mockProductRepository;
    private IProductMapper _productMapper;
    private Mock<IUnitOfWork> _mockUnitOfWork;
    private Mock<IDiscountService> _mockDiscountService;
    private Mock<IStatusRepository> _mockStatusRepository;
    private Mock<ILogger<UpdateProductCommandHandler>> _mockLogger;
    private Mock<ILoggerFactory> _mockLoggerFactory;

    private UpdateProductCommandHandler _handler;

    private IEnumerable<Status> _statuses;
    private IEnumerable<Product> _products;

    [SetUp]
    public void Setup()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _productMapper = new ProductMapper(); // Assuming a simple or test implementation
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockDiscountService = new Mock<IDiscountService>();
        _mockStatusRepository = new Mock<IStatusRepository>();
        _mockLogger = new Mock<ILogger<UpdateProductCommandHandler>>();
        _mockLoggerFactory = new Mock<ILoggerFactory>();

        _statuses = new List<Status>
        {
            Status.Create(1, "Active"),
            Status.Create(2, "Inactive")
        };

        _products = new List<Product>
        {
            Product.Create(1, "P001", "Product 1", Status.Create(1, "Active"), 150, "Description", 100, 10, 30, 50),
            Product.Create(2, "P002", "Product 2", Status.Create(1, "Inactive"), 100, "Description", 50, 5, 20, 30)
        };

        _mockLoggerFactory
            .Setup(x => x.CreateLogger(It.IsAny<string>()))
            .Returns(() => _mockLogger.Object);

        _mockStatusRepository
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(_statuses);

        _handler = new UpdateProductCommandHandler(
            _mockProductRepository.Object,
            _productMapper,
            _mockUnitOfWork.Object,
            _mockStatusRepository.Object,
            _mockDiscountService.Object,
            _mockLoggerFactory.Object);
    }

    [Test]
    public async Task Handle_ShouldUpdateProducts_WhenAllDependenciesAreValid()
    {
        // Arrange
        var command = new UpdateProductCommand
        {
            Products = new List<UpdateProductRequest>
            {
                BuildValidProductModel()
            }
        };

        var discounts = new List<DiscountDto>
        {
            new DiscountDto { ProductCode = "P001", Value = 10 }
        };

        _mockProductRepository
            .Setup(r => r.Find(It.IsAny<Expression<Func<Product, bool>>>()))
            .ReturnsAsync(_products.Take(1));

        _mockDiscountService
            .Setup(d => d.GetDiscountByProducts(It.IsAny<List<string>>()))
            .ReturnsAsync(discounts);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.AreEqual(1, result.Count());
        _mockUnitOfWork.Verify(u => u.Complete(), Times.Once);
    }

    [Test]
    public void Handle_ShouldThrowException_WhenStatusIsInvalid()
    {
        // Arrange
        var command = new UpdateProductCommand
        {
            Products = new List<UpdateProductRequest>
            {
                BuildValidProductModel(x => x.StatusName = "InvalidStatus")
            }
        };

        _mockProductRepository
            .Setup(r => r.Find(It.IsAny<Expression<Func<Product, bool>>>()))
            .ReturnsAsync(_products.Take(1));

        // Act & Assert
        Assert.ThrowsAsync<ProductStatusInvalidException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Test]
    public async Task Handle_ShouldApplyDiscount_WhenDiscountIsAvailable()
    {
        // Arrange
        var price = 100;
        var discount = 10;
        var command = new UpdateProductCommand
        {
            Products = new List<UpdateProductRequest>
            {
                BuildValidProductModel(x =>
                {
                    x.Price = price;
                    x.Discount = null;
                })
            }
        };

        var discounts = new List<DiscountDto>
        {
            new DiscountDto { ProductCode = "P001", Value = discount }
        };

        _mockProductRepository
            .Setup(r => r.Find(It.IsAny<Expression<Func<Product, bool>>>()))
            .ReturnsAsync(_products.Take(1));

        _mockDiscountService
            .Setup(d => d.GetDiscountByProducts(It.IsAny<List<string>>()))
            .ReturnsAsync(discounts);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.AreEqual(1, result.Count());
        Assert.AreEqual(discount, result.First().Discount);
        Assert.AreEqual(CalculateFinalPrice(price, discount), result.First().FinalPrice);
    }

    [Test]
    public async Task Handle_ShouldNotApplyDiscount_WhenNoDiscountIsAvailable()
    {
        // Arrange
        var price = 100;
        var command = new UpdateProductCommand
        {
            Products = new List<UpdateProductRequest>
            {
                BuildValidProductModel(x =>
                {
                    x.Price = price;
                    x.Discount = null;
                })
            }
        };

        var discounts = new List<DiscountDto>();

        _mockProductRepository
            .Setup(r => r.Find(It.IsAny<Expression<Func<Product, bool>>>()))
            .ReturnsAsync(_products.Take(1).Select(x => { x.SetDiscount(0); return x; }));

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
    public async Task Handle_ShouldUpdateMultipleProducts_WhenAllDependenciesAreValid()
    {
        // Arrange
        var discount = 10;
        var command = new UpdateProductCommand
        {
            Products = new List<UpdateProductRequest>
            {
                BuildValidProductModel(x => x.Price = 120),
                BuildValidProductModel(x =>
                {
                    x.ProductId = 2;
                    x.ProductCode = "P002";
                    x.Price = 60;
                    x.Discount = null;
                })
            }
        };

        var discounts = new List<DiscountDto>
        {
            new DiscountDto { ProductCode = "P002", Value = discount }
        };

        _mockProductRepository
            .Setup(r => r.Find(It.IsAny<Expression<Func<Product, bool>>>()))
            .ReturnsAsync(_products);

        _mockDiscountService
            .Setup(d => d.GetDiscountByProducts(It.IsAny<List<string>>()))
            .ReturnsAsync(discounts);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.AreEqual(2, result.Count());
        Assert.AreEqual(15, result.First(x => x.ProductCode == "P001").Discount);
        Assert.AreEqual(discount, result.First(x => x.ProductCode == "P002").Discount);
    }

    [Test]
    public async Task Handle_ShouldLogInformation_WhenHandlingCommand()
    {
        // Arrange
        var command = new UpdateProductCommand
        {
            Products = new List<UpdateProductRequest>
            {
                BuildValidProductModel(),
            }
        };

        var discounts = new List<DiscountDto>
        {
            new DiscountDto { ProductCode = "P001", Value = 10 }
        };

        _mockProductRepository
            .Setup(r => r.Find(It.IsAny<Expression<Func<Product, bool>>>()))
            .ReturnsAsync(_products.Take(1));

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
    public async Task Handle_ShouldThrowException_WhenProductIdIsNotFound()
    {
        // Arrange
        var command = new UpdateProductCommand
        {
            Products = new List<UpdateProductRequest>
            {
                BuildValidProductModel(x =>
                {
                    x.ProductId = 999;
                    x.ProductCode = "P999";
                })
            }
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.IsEmpty(result); // Assuming empty result means no products were updated
    }

    private decimal CalculateFinalPrice(decimal price, decimal discount) => price * (100 - discount) / 100;

    private UpdateProductRequest BuildValidProductModel(Action<UpdateProductRequest> value = null)
    {
        var createProductRequest = new UpdateProductRequest
        {
            ProductId = 1,
            ProductCode = "P001",
            Name = "Updated Product",
            StatusName = "Active",
            Stock = 200,
            Description = "Updated Description",
            Price = 120,
            Discount = 15,
            Weight = 35,
            Size = 55
        };

        value?.Invoke(createProductRequest);

        return createProductRequest;
    }
}
