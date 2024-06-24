using Microsoft.AspNetCore.Http;

namespace Shared.Middleware
{
    public class RequestTimingMiddleware
    {
        private readonly RequestDelegate _next;
        public RequestTimingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var start = DateTime.UtcNow;

            context.Response.OnStarting(() =>
            {
                var end = DateTime.UtcNow;
                var duration = end - start;
                File.AppendAllText("logs.txt", $"{context.Request.Path} - {duration.TotalMilliseconds} ms{Environment.NewLine}");
                return Task.CompletedTask;
            });

            await _next(context);
        }
    }
}
