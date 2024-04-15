using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JourneyHub.Api.Middlewares;

public class HealthCheckMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(next);

        try
        {
            await next(context).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred: {Message}", ex.Message);

            var exceptionDetails = GetExceptionDetails(ex);

            var problemDetails = new ProblemDetails
            {
                Status = exceptionDetails.Status,
                Type = exceptionDetails.Type,
                Title = exceptionDetails.Title,
                Detail = exceptionDetails.Detail,
            };

            if (exceptionDetails.Errors is not null)
            {
                problemDetails.Extensions["errors"] = exceptionDetails.Errors;
            }

            context.Response.StatusCode = exceptionDetails.Status;

            await context.Response.WriteAsJsonAsync(problemDetails).ConfigureAwait(false);
        }
    }

    public static IApplicationBuilder UseHealthChecks(this IApplicationBuilder applicationBuilder)
    {
        return applicationBuilder.UseHealthChecks("/health", new HealthCheckOptions
        {
            AllowCachingResponses = false,
            ResultStatusCodes =
             {
               [HealthStatus.Healthy] = StatusCodes.Status200OK,
               [HealthStatus.Degraded] = StatusCodes.Status206PartialContent,
               [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
             },
            ResponseWriter = WriteHealthCheckResponse
        });
    }

    private static Task WriteHealthCheckResponse(HttpContext httpCcontext, HealthReport healthReport)
    {
        httpCcontext.Response.ContentType = "application/json";

        var json = new JObject(
            new JProperty("status", healthReport.Status.ToString()),
            new JProperty("results", new JObject(healthReport.Entries.Select(pair =>
                new JProperty(pair.Key, new JObject(
                    new JProperty("status", pair.Value.Status.ToString()),
                    new JProperty("duration", pair.Value.Duration),
                    new JProperty("description", pair.Value.Description),
                    new JProperty("exception", pair.Value.Exception?.Message),
                    new JProperty("tags", pair.Value.Tags),
                    new JProperty("data", new JObject(pair.Value.Data.Select(
                        p => new JProperty(p.Key, p.Value))))
                    ))))));

        return httpCcontext.Response.WriteAsync(json.ToString(Formatting.Indented));
    }
}
