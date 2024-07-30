using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace StockSmart.Presentation.Controllers.Base;

public  class ApiControllerBase(IMediator mediator) : ControllerBase
{
    protected readonly IMediator _mediator = mediator;
}
