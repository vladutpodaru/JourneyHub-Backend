using JourneyHub.Api.Middlewares;
using JourneyHub.Application.Constants;
using Serilog;

namespace JourneyHub.Api.Extensions
{
    public static class MiddlewareExtensions
    {
        private static void UseMiddleware<TMiddleware>(this IApplicationBuilder app) where TMiddleware : IMiddleware
        {
            ArgumentNullException.ThrowIfNull(app);
            app.UseMiddleware<TMiddleware>();
        }

        public static WebApplication AddMiddlewares(this WebApplication app)
        {
            ArgumentNullException.ThrowIfNull(app);

            var environment = app.Environment;
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (environment.IsProduction())
            {
                app.UseCookiePolicy();
                app.UseHsts();
                app.UseMiddleware<SecurityHeadersMiddleware>();
                app.UseResponseCompression();
            }

            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();

            app.UseCors(Constans.CORSAllowUI);
            app.UseMiddleware<HealthCheckMiddleware>();
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            // app.UseAuthentication();
            // app.UseAuthorization();
            app.UseCors(Constans.CORSAllowUI);
            app.MapGraphQL(Constans.GraphQLEndpoint);

            app.Run();

            return app;
        }
    }
}
