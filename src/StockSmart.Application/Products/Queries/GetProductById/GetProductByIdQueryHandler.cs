using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StockSmart.Application.Common.Abstract;
using StockSmart.Application.Products.Mappers.Abstract;
using StockSmart.Domain.Common.Abstract;
using StockSmart.Domain.Exceptions;

namespace StockSmart.Application.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler(IProductRepository productRepository, IProductMapper productMapper, ILoggerFactory loggerFactory)
    : IQueryHandler<GetProductByIdQuery, ProductResponse>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IProductMapper _productMapper = productMapper;
    private readonly ILogger _logger = loggerFactory.CreateLogger<GetProductByIdQueryHandler>();

    public async Task<ProductResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Handling {nameof(GetProductByIdQueryHandler)} ProductId {request.ProductId}");

        var productEntity = await _productRepository.GetById(request.ProductId);
        if (productEntity == null)
        {
            throw new ProductNotFoundException("Product not found", request.ProductId);
        }
        return await _productMapper.ReverseMap(productEntity);
    }
}
