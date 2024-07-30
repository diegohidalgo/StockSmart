using System.Collections.Generic;
using StockSmart.Application.Common.Abstract;
using StockSmart.Application.Products.Queries.GetProductById;

namespace StockSmart.Application.Products.Command.CreateProduct;

public class CreateProductCommand : ICommand<IEnumerable<ProductResponse>>
{
    public IEnumerable<CreateProductRequest> Products { get; set; }
}
