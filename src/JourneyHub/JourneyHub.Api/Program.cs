using Serilog;
using JourneyHub.Api.Extensions;
using System.Globalization;

try
{
    var builder = WebApplication.CreateBuilder(args);

    #region Logger
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext()
        .Enrich.WithProperty("ApplicationName", typeof(Program).Assembly.GetName().Name)
        .Enrich.WithProperty("Environment", builder.Environment)
        .CreateLogger();

    builder.Host.UseSerilog(Log.Logger);
    #endregion

    #region Build & Run
    builder.AddWebApplicationBuilder();

    var app = builder.Build();
    app.AddWebApplication();
    #endregion
}
catch (Exception ex) when (Log.Logger == null || Log.Logger.GetType().Name == "SilentLogger")
{
    #region Log the exception
    var outputTemplateType = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}";

    Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console(outputTemplate: outputTemplateType, formatProvider: CultureInfo.InvariantCulture)
            .CreateLogger();

    Log.Fatal(ex, "Host terminated unexpectedly");
    #endregion
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program
{
    protected Program() { }
}