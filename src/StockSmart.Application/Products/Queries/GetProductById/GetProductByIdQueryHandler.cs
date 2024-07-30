using System.Threading;
using System.Threading.Tasks;
using MediatR;
using StockSmart.Application.Products.Mappers.Abstract;
using StockSmart.Domain.Common.Abstract;
using StockSmart.Domain.Exceptions;

namespace StockSmart.Application.Products.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductMapper _productMapper;

        public GetProductByIdQueryHandler(IProductRepository productRepository, IProductMapper productMapper)
        {
            _productRepository = productRepository;
            _productMapper = productMapper;
        }

        public async Task<ProductResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var productEntity = await _productRepository.GetById(request.ProductId);
            if (productEntity == null)
            {
                throw new ProductNotFoundException("Product not found", request.ProductId);
            }
            return await _productMapper.ReverseMap(productEntity);
        }
    }
}
