using System.Collections.Generic;
using System.Threading.Tasks;
using StockSmart.Application.Common.Abstract;
using StockSmart.Application.Products.Command.CreateProduct;
using StockSmart.Application.Products.Queries.GetProductById;
using StockSmart.Domain.Entities;

namespace StockSmart.Application.Products.Mappers.Abstract;

public interface IProductMapper : IMapper
{
    Task<ProductResponse> ReverseMap(Product product);
    Task<IEnumerable<ProductResponse>> ReverseMapList(IEnumerable<Product> products);
    Task<IEnumerable<Product>> MapList(IEnumerable<CreateProductRequest> productRequests);
}
