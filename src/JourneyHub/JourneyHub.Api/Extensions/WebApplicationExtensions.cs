using JourneyHub.Api.Middlewares.Extensions;
using JourneyHub.Application.Constants;
using Serilog;

namespace JourneyHub.Api.Extensions
{
    public static class WebApplicationExtensions
    {
        public static WebApplication AddWebApplication(this WebApplication app)
        {
            ArgumentNullException.ThrowIfNull(app);

            var environment = app.Environment;
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (environment.IsProduction())
            {
                app.UseHsts();
                app.UseCookiePolicy();
                app.UseSecurityHeaders();
                app.UseResponseCompression();
            }

            app.UseCors(Constants.CORSAllowUI);
            app.UseHttpsRedirection();

            app.UseRequestContextLogging();
            app.UseSerilogRequestLogging();

            app.UseGlobalExceptionHandler();
            // app.UseHealthCheck();

            // app.UseAuthentication();
            // app.UseAuthorization();
            app.MapGraphQL(Constants.GraphQLEndpoint);

            app.Run();

            return app;
        }
    }
}
