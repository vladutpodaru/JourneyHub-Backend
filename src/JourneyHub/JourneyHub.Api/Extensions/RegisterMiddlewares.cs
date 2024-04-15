using JourneyHub.Api.Middlewares.Extensions;
using Serilog;

namespace JourneyHub.Api.Extensions
{
    public static class RegisterMiddlewares
    {
        private static readonly string GraphQLEndpoint = "/journey-hub-api";

        public static WebApplication RegisterAllMiddlewares(this WebApplication app, string CORSAllowUI)
        {
            var environment = app.Environment;
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (environment.IsProduction())
            {
                app.UseCookiePolicy();
                app.UseSecurityHeaders();
                app.UseHsts();
                app.UseResponseCompression();
            }

            app.UseCors(CORSAllowUI);
            app.UseHealthChecks();
            app.UseExceptionHandling();
            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            // app.UseAuthentication();
            // app.UseAuthorization();
            app.MapGraphQL(GraphQLEndpoint);

            return app;
        }
    }
}
