using System.ComponentModel.DataAnnotations;
using StockSmart.Application.Common.Abstract;

namespace StockSmart.Application.Products.Queries.GetProductById;

public class GetProductByIdQuery(int productId) : IQuery<ProductResponse>
{
    [Required]
    [Range(1, int.MaxValue)]
    public int ProductId { get; private set; } = productId;
}
