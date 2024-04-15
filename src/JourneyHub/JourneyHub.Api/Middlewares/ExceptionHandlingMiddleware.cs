using Microsoft.AspNetCore.Mvc;

namespace JourneyHub.Api.Middlewares
{
    public class ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) : IMiddleware
    {
        internal record ExceptionDetails(
            int Status,
            string Type,
            string Title,
            string Detail,
            IEnumerable<object>? Errors
        );

        private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

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

        private static ExceptionDetails GetExceptionDetails(Exception exception)
        {
            return exception switch
            {
                ValidationException validationException => new ExceptionDetails(
                    StatusCodes.Status400BadRequest,
                    "ValidationFailure",
                    "Validation error",
                    "One or more validation errors has occurred",
                    validationException.Errors),
                _ => new ExceptionDetails(
                    StatusCodes.Status500InternalServerError,
                    "ServerError",
                    "Server error",
                    "An unexpected error has occurred",
                    null)
            };
        }
    }
}
