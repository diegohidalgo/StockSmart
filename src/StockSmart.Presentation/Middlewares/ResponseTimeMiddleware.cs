using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace StockSmart.Presentation.Middlewares
{
    public class ResponseTimeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ResponseTimeMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<ResponseTimeMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            var watcher = Stopwatch.StartNew();
            await _next.Invoke(context);
            watcher.Stop();
            _logger.LogInformation($"Response time of {context.Request.Method} {context.Request.Path} is {watcher.ElapsedMilliseconds}ms");
        }
    }
}
