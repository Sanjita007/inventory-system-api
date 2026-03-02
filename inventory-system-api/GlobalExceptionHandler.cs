using inventory_system_api.Application.IRepository;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Text;

public class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandler> logger, IErrorLogRepository errorLog) : IExceptionHandler
{
    public Task HandleAsync(HttpContext context, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Unhandled exception occurred");

        httpContext.Response.StatusCode = exception switch
        {
            ApplicationException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };
        try
        {
            string bodyStr = "";
            using (StreamReader reader
                  = new(httpContext.Request.Body, Encoding.UTF8, true, 1024, true))
            {
                bodyStr = await reader.ReadToEndAsync();
            }

            await errorLog.AddErrorLog(new inventory_system_api.Application.Models.ErrorLog()
            {
                ErrorMessage = exception.StackTrace??exception.Message,
                RequestBody = bodyStr,
                RequestHeader = string.Join("\n", httpContext.Request.Headers.Select(x => $"{x.Key}: {x.Value}")),
                RequestMethod = httpContext.Request.Method,
                RequestPath = httpContext.Request.Path,
                DateTimeUtc = DateTime.UtcNow

            });            
        }
        catch (Exception dbEx)
        {
            
           Console.WriteLine(dbEx.Message);
        }

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Type = exception.GetType().Name,
                Title = "An error occured",
                Detail = exception.Message
            }
        });
    }
}