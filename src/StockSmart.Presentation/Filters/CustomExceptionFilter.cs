using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StockSmart.Domain.Exceptions;

namespace StockSmart.Presentation.Filters
{
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

        private object BuildResponseMessage(StockSmartException exception)
        {
            return JsonSerializer.Serialize(
                new ProblemDetailResponse(exception.GetType().ToString(), exception.Message, exception.StatusCode, JsonSerializer.Serialize(exception.Value)));
        }
    }

    class ProblemDetailResponse
    {
        public ProblemDetailResponse(string type, string detail, int status, object value)
        {
            this.Type = type;
            this.Detail = detail;
            this.Status = status;
            this.Value = value;
        }

        public string? Type { get; set; }
        public string? Detail { get; set; }
        public int? Status { get; set; }
        public object? Value { get; set; }
    }
}
