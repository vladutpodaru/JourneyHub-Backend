using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Presentation
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services, ILogger logger)
        {
            services.AddGraphQLServer();
            //.AddQueryType<Query>();

            logger.LogInformation("{Project} services registered", "Presentation");

            return services;
        }
    }
}
