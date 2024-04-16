namespace JourneyHub.Application.Interfaces;

#pragma warning disable S2326 
// Generic T is used to define interface in methods
public interface ILoggerService<T>
{
    void LogErrorException(Exception exception, string message);
    void LogError(string message);
    void LogInformation(string message);
}
