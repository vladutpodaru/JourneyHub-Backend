namespace JourneyHub.Api.Middlewares.Extensions;

public static class MiddlewareExtensions
{
    public static void UseSecurityHeaders(this IApplicationBuilder app)
    {
        app.UseMiddleware<SecurityHeaders>();
    }

    public static void UseRequestContextLogging(this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestContextLogging>();
    }

    public static void UseHealthCheck(this IApplicationBuilder app)
    {
        app.UseMiddleware<HealthCheck>();
    }

    public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(options => { });
    }

    private static void UseMiddleware<TMiddleware>(this IApplicationBuilder app) where TMiddleware : IMiddleware
    {
        ArgumentNullException.ThrowIfNull(app);
        app.UseMiddleware<TMiddleware>();
    }
}
