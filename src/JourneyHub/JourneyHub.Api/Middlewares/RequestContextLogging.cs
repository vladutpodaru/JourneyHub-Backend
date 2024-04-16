using Serilog.Context;

namespace JourneyHub.Api.Middlewares;

public class RequestContextLogging(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;
    private const string CorrelationIdHeaderName = "X-Correlation-Id";

    public Task Invoke(HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(httpContext);

        using (LogContext.PushProperty("CorrelationId", GetCorrelationId(httpContext)))
        {
            return _next(httpContext);
        }
    }

    private static string GetCorrelationId(HttpContext httpContext)
    {
        httpContext.Request.Headers.TryGetValue(CorrelationIdHeaderName, out var correlationId);

        return correlationId.FirstOrDefault() ?? httpContext.TraceIdentifier;
    }
}