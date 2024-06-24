using Microsoft.AspNetCore.Http;

namespace Shared.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _file = "exceptions.txt";

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            var errorMessage =
                $"[{DateTime.Now}] {exception.Message}{Environment.NewLine}{exception.StackTrace}{Environment.NewLine}";
            await File.AppendAllTextAsync(_file, errorMessage);

            await context.Response.WriteAsync(new
            {
                context.Response.StatusCode,
                Message = "Internal Server Error. The error has been logged."
            }.ToString());
        }
    }
}
