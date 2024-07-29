using MediatR;

namespace StockSmart.Application.Products.Queries.GetProductById
{
    public class GetProductByIdQuery : IRequest<ProductResponse>
    {
        public GetProductByIdQuery(int productId)
        {
            ProductId = productId;
        }

        public int ProductId { get; private set; }
    }
}
