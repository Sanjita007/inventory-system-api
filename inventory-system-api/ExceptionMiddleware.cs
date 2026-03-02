using System.Text.Json;
using System.Text;
using System.IO;
using System.Linq;
using inventory_system_api.Models;
using inventory_system_api.Application.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using inventory_system_api.Application.Models;

namespace inventory_system_api.Middleware
{
    /// <summary>
    /// This is the new exception handling middleware that will catch all unhandled exceptions in the request pipeline and return a standardized error response. It also logs the exception details for debugging purposes.
    /// </summary>
    public sealed class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var status = StatusCodes.Status500InternalServerError;
            var response = new inventory_system_api.Application.Models.ErrorResponse
            {
                StatusCode = status,
                Message = "An unexpected error occurred.",
                TraceId = context.TraceIdentifier
            };
            // Try to read the request body safely (buffering must be enabled by upstream middleware)
            string bodyStr = string.Empty;
            try
            {
                context.Request.Body.Position = 0;
            }
            catch { /* ignore if not seekable */ }

            try
            {
                using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
                bodyStr = await reader.ReadToEndAsync();
                // Reset position so other middleware can read the body
                try { context.Request.Body.Position = 0; } catch { }
            }
            catch { /* swallow - reading body is best-effort */ }

            // Log error to repository if available via DI -- avoid direct static references.
            try
            {
                var errorRepo = context.RequestServices.GetService<IErrorLogRepository>();
                if (errorRepo != null)
                {
                    await errorRepo.AddErrorLog(new ErrorLog
                    {
                        ErrorMessage = exception.StackTrace ?? exception.Message,
                        RequestBody = bodyStr,
                        RequestHeader = string.Join("\n", context.Request.Headers.Select(x => $"{x.Key}: {x.Value}")),
                        RequestMethod = context.Request.Method,
                        RequestPath = context.Request.Path,
                        DateTimeUtc = DateTime.UtcNow
                    });
                }
            }
            catch (Exception logEx)
            {
                // Ensure that error logging does not throw
                // We log the failure through middleware logger if possible
                try { var logger = context.RequestServices.GetService<ILogger<ExceptionMiddleware>>(); logger?.LogError(logEx, "Failed writing error log"); } catch { }
            }

            var payload = JsonSerializer.Serialize(response);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = status;
            await context.Response.WriteAsync(payload);
            return;
        }
    }
}