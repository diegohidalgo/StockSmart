using System.Collections.Generic;
using StockSmart.Application.Common.Abstract;
using StockSmart.Application.Products.Queries.GetProductById;

namespace StockSmart.Application.Products.Command.UpdateProduct
{
    public class UpdateProductCommand : ICommand<IEnumerable<ProductResponse>>
    {
        public IEnumerable<UpdateProductRequest> Products { get; set; }
    }
}
