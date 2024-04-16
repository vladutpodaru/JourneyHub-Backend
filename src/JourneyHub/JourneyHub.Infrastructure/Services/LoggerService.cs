using JourneyHub.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace JourneyHub.Infrastructure.Services;

public class LoggerService<T>(ILogger<T> logger) : ILoggerService<T>
{
    private readonly ILogger<T> _logger = logger;

    public void LogErrorException(Exception exception, string message)
    {
        LoggerMessage.Define<string>(LogLevel.Error, new EventId(), message)(_logger, $"{message}", exception);
    }

    public void LogError(string message)
    {
        LoggerMessage.Define<string>(LogLevel.Error, new EventId(), message)(_logger, $"{message}", null);
    }

    public void LogInformation(string message)
    {
        LoggerMessage.Define<string>(LogLevel.Information, new EventId(), message)(_logger, $"{message}", null);
    }
}
