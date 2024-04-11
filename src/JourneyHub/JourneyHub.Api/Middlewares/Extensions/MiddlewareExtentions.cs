namespace JourneyHub.Api.Middlewares.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app)
        {
            return app == null ? throw new ArgumentNullException(nameof(app)) : app.UseMiddleware<SecurityHeaders>();
        }

        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
        {
            return app == null ? throw new ArgumentNullException(nameof(app)) : app.UseMiddleware<ExceptionHandling>();
        }
    }
}
