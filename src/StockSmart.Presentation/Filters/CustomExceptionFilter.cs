using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StockSmart.Domain.Exceptions;

namespace StockSmart.Presentation.Filters;

public class CustomExceptionFilter : IActionFilter, IOrderedFilter
{
    public int Order => int.MaxValue - 10;

    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is StockSmartException stockSmartException)
        {
            context.Result = new ObjectResult(BuildResponseMessage(stockSmartException))
            {
                StatusCode = stockSmartException.StatusCode
            };

            context.ExceptionHandled = true;
        }
    }

    private object BuildResponseMessage(StockSmartException exception) => JsonSerializer.Serialize(
            new ProblemDetailResponse(exception.GetType().ToString(), exception.Message, exception.StatusCode, JsonSerializer.Serialize(exception.Value)));
}

class ProblemDetailResponse(string type, string detail, int status, object value)
{
    public string? Type { get; set; } = type;
    public string? Detail { get; set; } = detail;
    public int? Status { get; set; } = status;
    public object? Value { get; set; } = value;
}
