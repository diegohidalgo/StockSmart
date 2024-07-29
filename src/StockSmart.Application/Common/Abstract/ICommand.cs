using MediatR;

namespace StockSmart.Application.Common.Abstract
{
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
}
