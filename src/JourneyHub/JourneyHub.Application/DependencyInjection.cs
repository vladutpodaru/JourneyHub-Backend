﻿using FluentValidation;
using JourneyHub.Application.Behaviors;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace JourneyHub.Application
{
    public static class DependencyInjection
    {
        public static void AddApplication(IServiceCollection services) 
        {
            var assembly = typeof(DependencyInjection).Assembly;

            services.AddMediatR(configuration => 
            {
                configuration.RegisterServicesFromAssembly(assembly); //.AddMediatorHandlers(services);
                configuration.AddOpenBehavior(typeof(RequestLoggingPipelineBehavior<,>));
            });

            services.AddValidatorsFromAssembly(assembly);
        }
    }
}
