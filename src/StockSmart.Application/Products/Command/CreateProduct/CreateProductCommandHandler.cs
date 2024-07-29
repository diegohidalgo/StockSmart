using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StockSmart.Application.Common.Abstract;
using StockSmart.Application.Products.Mappers.Abstract;
using StockSmart.Application.Products.Queries.GetProductById;
using StockSmart.Application.Services;
using StockSmart.Domain.Common.Abstract;
using StockSmart.Domain.Exceptions;

namespace StockSmart.Application.Products.Command.CreateProduct
{
    public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, IEnumerable<ProductResponse>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IStatusRepository _statusRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductMapper _productMapper;
        private readonly IDiscountService _discountService;
        private readonly ILogger _logger;

        public CreateProductCommandHandler(
            IProductRepository productRepository,
            IProductMapper productMapper,
            IUnitOfWork unitOfWork,
            IDiscountService discountService,
            IStatusRepository statusRepository,
            ILoggerFactory loggerFactory)
        {
            _productRepository = productRepository;
            _productMapper = productMapper;
            _unitOfWork = unitOfWork;
            _discountService = discountService;
            _statusRepository = statusRepository;
            _logger = loggerFactory.CreateLogger<CreateProductCommandHandler>();
        }

        public async Task<IEnumerable<ProductResponse>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling {nameof(CreateProductCommandHandler)}");
            var productsToSave = await _productMapper.MapList(request.Products);

            var statuses = await _statusRepository.GetAllAsync();
            _logger.LogInformation($"Status found {statuses.Count()}");

            var statusDictionary = statuses.Distinct().ToDictionary(x => x.Name);

            var productDiscounts = await _discountService.GetDiscountByProducts(productsToSave.Select(x => x.ProductCode).ToList());

            var productDiscountsDictionary = productDiscounts.Distinct().ToDictionary(x => x.ProductCode);

            foreach (var product in productsToSave)
            {
                if (!statusDictionary.TryGetValue(product.Status.Name, out var productStatus))
                {
                    throw new ProductStatusInvalidException("Product status not found", product.Status.Name);
                }
                product.UpdateStatus(productStatus.StatusId);

                if (productDiscountsDictionary.TryGetValue(product.ProductCode, out var productDiscount))
                {
                    product.SetDiscount(productDiscount.Value);
                }
            }

            await _productRepository.AddRange(productsToSave);

            _logger.LogInformation($"Creating products {productsToSave.Count()}");

            await _unitOfWork.Complete();

            return await _productMapper.ReverseMapList(productsToSave);
        }
    }
}
