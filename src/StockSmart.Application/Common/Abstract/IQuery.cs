using MediatR;

namespace StockSmart.Application.Common.Abstract;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}
