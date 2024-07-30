using MediatR;

namespace StockSmart.Application.Common.Abstract;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
where TCommand : ICommand<TResponse>
{
}
