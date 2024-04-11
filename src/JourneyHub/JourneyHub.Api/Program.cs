using JourneyHub.Infrastructure;
using JourneyHub.Application;
using JourneyHub.Presentation;
using Serilog;

const string AllowedOrigin = "allowedJourneyHubOrigin";
const string GraphQLEndpoint = "/journey-hub-api";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(option => 
    option.AddPolicy(AllowedOrigin, builder =>
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader()));

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// IOC registration
builder.Services
    .AddInfrastructure()
    .AddApplication()
    .AddPresentation();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseCors(AllowedOrigin);
app.MapGraphQL(GraphQLEndpoint);
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.Run();
