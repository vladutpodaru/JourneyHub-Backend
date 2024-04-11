using Serilog;
using JourneyHub.Api.Extensions;

try
{
    const string CORSAllowUI = "CORSAllowUI";

    var builder = WebApplication.CreateBuilder(args);

    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext()
        .Enrich.WithProperty("ApplicationName", typeof(Program).Assembly.GetName().Name)
        .Enrich.WithProperty("Environment", builder.Environment)
        .CreateLogger();

    builder.Host.UseSerilog(Log.Logger);
    builder.RegisterAllServices(CORSAllowUI)
           .Build()
           .RegisterAllMiddlewares(CORSAllowUI)
           .Run();
}
catch (Exception e)
{
    if (Log.Logger == null || Log.Logger.GetType().Name == "SilentLogger")
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger();
    }

    Log.Fatal(e, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}