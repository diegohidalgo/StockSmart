using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StockSmart.Application.Common.Abstract;
using StockSmart.Application.Dto;
using StockSmart.Application.Products.Mappers.Abstract;
using StockSmart.Application.Products.Queries.GetProductById;
using StockSmart.Application.Services;
using StockSmart.Domain.Common.Abstract;
using StockSmart.Domain.Exceptions;

namespace StockSmart.Application.Products.Command.UpdateProduct
{
    public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, IEnumerable<ProductResponse>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IStatusRepository _statusRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductMapper _productMapper;
        private readonly IDiscountService _discountService;
        private readonly ILogger _logger;

        public UpdateProductCommandHandler(
            IProductRepository productRepository,
            IProductMapper productMapper,
            IUnitOfWork unitOfWork,
            IStatusRepository statusRepository,
            IDiscountService discountService,
            ILoggerFactory loggerFactory)
        {
            _productRepository = productRepository;
            _productMapper = productMapper;
            _unitOfWork = unitOfWork;
            _statusRepository = statusRepository;
            _discountService = discountService;
            _logger = loggerFactory.CreateLogger<UpdateProductCommandHandler>();
        }

        public async Task<IEnumerable<ProductResponse>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling {nameof(UpdateProductCommandHandler)}");

            var products = await _productRepository.Find(t => request.Products.Select(x => x.ProductId).Contains(t.ProductId));
            _logger.LogInformation($"Products found {products.Count()}");

            var productsDictionary = products.Distinct().ToDictionary(x => x.ProductId);

            var statuses = await _statusRepository.GetAllAsync();
            _logger.LogInformation($"Status found {statuses.Count()}");

            var statusDictionary = statuses.Distinct().ToDictionary(x => x.Name);

            var productDiscounts = await _discountService.GetDiscountByProducts(products.Select(x => x.ProductCode).ToList());

            var productDiscountsDictionary = productDiscounts.Distinct().ToDictionary(x => x.ProductCode);

            foreach (var requestProduct in request.Products)
            {
                if (!productsDictionary.TryGetValue(requestProduct.ProductId, out var product))
                {
                    continue;
                }

                if (!statusDictionary.TryGetValue(requestProduct.StatusName, out var productStatus))
                {
                    throw new ProductStatusInvalidException("Product status not found", requestProduct.StatusName);
                }

                DiscountDto productDiscount = null;
                if (requestProduct.Discount is null)
                {
                    productDiscount = productDiscountsDictionary.GetValueOrDefault(requestProduct.ProductCode);
                }

                product.Update(requestProduct.ProductCode, requestProduct.Name, productStatus.StatusId, null, requestProduct.Stock,
                    requestProduct.Description, requestProduct.Price, requestProduct.Discount ?? productDiscount?.Value, requestProduct.Weight, requestProduct.Size);
            }

            _logger.LogInformation($"Updating products");
            await _unitOfWork.Complete();

            return await _productMapper.ReverseMapList(products);
        }
    }
}
