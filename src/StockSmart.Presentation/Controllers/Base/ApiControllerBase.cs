using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace StockSmart.Presentation.Controllers.Base
{
    public  class ApiControllerBase : ControllerBase
    {
        protected readonly IMediator _mediator;

        public ApiControllerBase(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}
