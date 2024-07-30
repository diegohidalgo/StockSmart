using MediatR;

namespace StockSmart.Application.Common.Abstract;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
where TQuery : IQuery<TResponse>
{
}
