using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JourneyHub.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("OptHandler");
            // services.AddDbContextPool<OptContext>(optionsBuilder => optionsBuilder.UseNpgsql(connectionString, options => options.EnableRetryOnFailure()).EnableSensitiveDataLogging(true));
        }
    }
}
