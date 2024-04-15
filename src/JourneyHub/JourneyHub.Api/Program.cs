using Serilog;
using JourneyHub.Api.Extensions;

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
    builder.AddServices();

    var app = builder.Build();
    app.AddMiddlewares();
    #endregion
}
finally
{
    Log.CloseAndFlush();
}