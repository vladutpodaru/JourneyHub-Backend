using Application;
using Infrastructure;
using Presentation;
using Serilog;
using Serilog.Extensions.Logging;

var logger = Log.Logger = new LoggerConfiguration()
  .Enrich.FromLogContext()
  .WriteTo.Console()
  .CreateLogger();

logger.Information("Starting web host");

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Logger: TODO: to put configuration in appsettings
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

var programLogger = new SerilogLoggerFactory(logger).CreateLogger<Program>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddInfrastructure(configuration, programLogger)
                .AddApplication(programLogger)
                .AddPresentation(programLogger);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.MapBananaCakePop("/endpoint/graphql/ui");
app.Run();
