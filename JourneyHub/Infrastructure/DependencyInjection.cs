using Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, ILogger logger)
        {
            var connectionString = configuration.GetConnectionString("JourneyHub") ?? throw new ArgumentNullException();

            services.AddDbContextPool<JourneyDbContext>(options => options.UseNpgsql(connectionString, options =>
                                                        options.EnableRetryOnFailure()).EnableSensitiveDataLogging(true));

            //services.AddMemoryCache();
            //services.AddTransient<IUnitOfWork, UnitOfWork>();
            //services.AddScoped<JourneyDbContext, CommandDbContext>();
            //services.AddScoped<JourneyDbContext, QueryDbContext>();
            //services.AddSingleton<PublishDomainEventsInterceptor>();
            //services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            //services.AddTransient(typeof(IQueryGenericRepository<>), typeof(QueryGenericRepository<>));
            //services.Decorate(typeof(IQueryGenericRepository<>), typeof(CacheRepository<>));

            logger.LogInformation("{Project} services registered", "Infrastructure");

            return services;
        }
    }
}
