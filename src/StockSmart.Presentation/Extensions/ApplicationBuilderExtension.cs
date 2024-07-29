using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using StockSmart.Presentation.Middlewares;

namespace StockSmart.Presentation.Extensions
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder UseResponseTimeMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ResponseTimeMiddleware>();
        }
    }
}
