using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, ILogger logger)
        {
            var assembly = typeof(DependencyInjection).Assembly;

            services.AddMediatR(confiuration => confiuration.RegisterServicesFromAssembly(assembly));
            services.AddValidatorsFromAssembly(assembly);

            logger.LogInformation("{Project} services registered", "Application");

            return services;
        }
    }
}
