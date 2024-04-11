using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json.Linq;
using System.Xml;

namespace JourneyHub.Api.Extensions
{
    public static class RegisterHealthChecks
    {
        public static IApplicationBuilder UseHealthChecks(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseHealthChecks("/health", new HealthCheckOptions
            {
                AllowCachingResponses = false,
                ResultStatusCodes =
             {
               [HealthStatus.Healthy] = StatusCodes.Status200OK,
               [HealthStatus.Degraded] = StatusCodes.Status200OK,
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

            return httpCcontext.Response.WriteAsync(json.ToString((Newtonsoft.Json.Formatting)Formatting.Indented));
        }
    }
}
