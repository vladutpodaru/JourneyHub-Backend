using JourneyHub.Application.DTOs;
using JourneyHub.Application.Interfaces;
using MediatR;
using Serilog.Context;

namespace JourneyHub.Application.Behaviors;
internal sealed class RequestLoggingPipelineBehavior<TRequest, TResponse>(ILoggerService<RequestLoggingPipelineBehavior<TRequest, TResponse>> logger) 
    : IPipelineBehavior<TRequest, TResponse> where TRequest : class where TResponse : Result
{
    private readonly ILoggerService<RequestLoggingPipelineBehavior<TRequest, TResponse>> _logger = logger;

    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        string requestName = typeof(TRequest).Name;

        _logger.LogInformation($"Processing request {requestName}");

        TResponse result = await next().ConfigureAwait(false);

        if (result.IsSuccess)
        {
            _logger.LogInformation($"Completed request {requestName}");
        }
        else
        {
            using (LogContext.PushProperty("Error", result.Error, true))
            {
                _logger.LogError($"Completed request {requestName} with error");
            }
        }

        return result;
    }
}
