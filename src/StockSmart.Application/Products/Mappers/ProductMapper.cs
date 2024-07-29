using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockSmart.Application.Products.Command.CreateProduct;
using StockSmart.Application.Products.Mappers.Abstract;
using StockSmart.Application.Products.Queries.GetProductById;
using StockSmart.Domain.Entities;

namespace StockSmart.Application.Products.Mappers
{
    public class ProductMapper : IProductMapper
    {
        public async Task<ProductResponse> ReverseMap(Product product)
            => new ProductResponse
            {
                ProductId = product.ProductId,
                ProductCode = product.ProductCode,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Discount = product.Discount,
                FinalPrice = product.FinalPrice,
                StatusName = product.Status?.Name,
                Stock = product.Stock,
                Weight = product.Weight,
                Size = product.Size,
            };

        public async Task<IEnumerable<ProductResponse>> ReverseMapList(IEnumerable<Product> products)
            => products
                .Select(async product => await ReverseMap(product))
                .Select(x => x.Result)
                .ToList();

        public async Task<Product> Map(CreateProductRequest productRequest) => Product.Create
        (
            0,
            productRequest.ProductCode,
            productRequest.Name,
            Status.Create(0, productRequest.StatusName),
            productRequest.Stock,
            productRequest.Description,
            productRequest.Price,
            default,
            productRequest.Weight,
            productRequest.Size
        );

        public async Task<IEnumerable<Product>> MapList(IEnumerable<CreateProductRequest> productRequests) =>
            productRequests
                    .Select(async productRequest => await Map(productRequest))
                    .Select(x => x.Result)
                    .ToList();
    }
}
