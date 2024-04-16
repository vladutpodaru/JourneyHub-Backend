using JourneyHub.Application.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace JourneyHub.Api.Middlewares;

internal sealed class ExceptionDetails
{
    public int? StatusCode { get; set; }
    public string? Type { get; set; }
    public string? Title { get; set; }
    public string? Detail { get; set; }
    public string? Instance { get; set; }
}

public class GlobalExceptionHandler(ILoggerService<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private readonly ILoggerService<GlobalExceptionHandler> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(httpContext);
        ArgumentNullException.ThrowIfNull(exception);

        var result = GetExceptionDetails(_logger, httpContext, exception);

        await httpContext.Response.WriteAsJsonAsync(result, cancellationToken: cancellationToken).ConfigureAwait(false);

        return true;
    }

    private static ExceptionDetails GetExceptionDetails(ILoggerService<GlobalExceptionHandler> _logger, HttpContext httpContext, Exception exception)
    {
        ExceptionDetails result;

        // Need to define more exceptions
        switch (exception)
        {
            case ArgumentNullException argumentNullException:
                result = new ExceptionDetails
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Type = argumentNullException.GetType().Name,
                    Title = "An unexpected error occurred",
                    Detail = argumentNullException.Message,
                    Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                };
                _logger.LogErrorException(exception, $"Exception occured : {argumentNullException.Message}");
                break;
            default:
                result = new ExceptionDetails
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Type = exception.GetType().Name,
                    Title = "An unexpected error occurred",
                    Detail = exception.Message,
                    Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
                };
                _logger.LogErrorException(exception, $"Exception occured : {exception.Message}");
                break;
        }

        return result;
    }
}