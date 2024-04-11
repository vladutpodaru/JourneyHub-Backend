using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace JourneyHub.Api.Middlewares
{
    public class ExceptionHandling(ILogger<ExceptionHandling> logger) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                logger.LogError("{ExceptionMessage}", e.Message);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                ProblemDetails problem = new()
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Type = "Exception",
                    Title = "Error",
                    Detail = e.Message
                };

                string problemJson = JsonSerializer.Serialize(problem);

                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(problemJson);
            }
        }
    }
}
