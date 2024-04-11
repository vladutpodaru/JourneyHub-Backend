using HotChocolate.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace JourneyHub.Presentation
{
    public static class DependencyInjection
    {
        public static void AddPresentation(this IServiceCollection services)
        {
            services.AddGraphQLServer()
                    .AddQueryType()
                    .AddMutationType()
                    .AddSubscriptionType();
        }
    }
}
