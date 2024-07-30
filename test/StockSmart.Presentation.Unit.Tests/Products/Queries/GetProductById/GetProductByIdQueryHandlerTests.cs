using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using StockSmart.Application.Products.Mappers;
using StockSmart.Application.Products.Mappers.Abstract;
using StockSmart.Application.Products.Queries.GetProductById;
using StockSmart.Domain.Common.Abstract;
using StockSmart.Domain.Entities;
using StockSmart.Domain.Exceptions;

namespace StockSmart.Application.Unit.Tests.Products.Queries.GetProductById
{
    [TestFixture]
    public class GetProductByIdQueryHandlerTests
    {
        private Mock<IProductRepository> _mockProductRepository;
        private IProductMapper _productMapper;
        private GetProductByIdQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _productMapper = new ProductMapper();
            _handler = new GetProductByIdQueryHandler(
                _mockProductRepository.Object,
                _productMapper);
        }

        [Test]
        public async Task Handle_ShouldReturnProductResponse_WhenProductExists()
        {
            // Arrange
            var productId = 1;
            var productEntity = Product.Create(productId, "P001", "Product 1", Status.Create(1, "Active"), 150, "Description", 100, 10, 30, 50);
            var productResponse = new ProductResponse { ProductId = productId, Name = "Product 1" };

            _mockProductRepository
                .Setup(r => r.GetById(productId))
                .ReturnsAsync(productEntity);

            var query = new GetProductByIdQuery(productId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.AreEqual(productResponse.ProductId, result.ProductId);
            Assert.AreEqual(productResponse.Name, result.Name);
            _mockProductRepository.Verify(r => r.GetById(productId), Times.Once);
        }

        [Test]
        public void Handle_ShouldThrowProductNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = 1;

            _mockProductRepository
                .Setup(r => r.GetById(productId))
                .ReturnsAsync((Product)null);

            var query = new GetProductByIdQuery(productId);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ProductNotFoundException>(() => _handler.Handle(query, CancellationToken.None));
            Assert.AreEqual("Product not found", ex.Message);
            Assert.AreEqual(productId, ex.Value);
            _mockProductRepository.Verify(r => r.GetById(productId), Times.Once);
        }
    }
}
