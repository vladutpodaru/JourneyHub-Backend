using JourneyHub.Api.Middlewares;
using Microsoft.AspNetCore.CookiePolicy;

namespace JourneyHub.Api.Extensions
{
    public static class RegisterServices
    {
        public static WebApplicationBuilder RegisterAllServices(this WebApplicationBuilder builder, string CORSAllowUI)
        {
            var services = builder.Services;
            var configuration = builder.Configuration;
            var environment = builder.Environment;

            // DbContext PostGres
            // var connectionString = configuration.GetConnectionString("OptHandler");
            // services.AddDbContextPool<OptContext>(optionsBuilder => optionsBuilder.UseNpgsql(connectionString, options => options.EnableRetryOnFailure()).EnableSensitiveDataLogging(true));

            // Add Strict-Transport-Security HTTP header  
            services.AddHsts(x =>
            {
                x.Preload = true;
                x.IncludeSubDomains = true;
            });

            // Add secure cookie policies
            services.AddCookiePolicy(options =>
            {
                options.HttpOnly = HttpOnlyPolicy.Always;
                options.Secure = CookieSecurePolicy.Always;
            });

            // Add response compression
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
            });

            // Add CORS
            services.AddCors(options =>
            {
                string[] allowCustomHeaders = ["Content-Disposition"];
                options.AddPolicy(CORSAllowUI,
                builder =>
                {
                    builder.AllowAnyHeader()
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .WithExposedHeaders(allowCustomHeaders);
                });
            });
            services.AddHealthChecks();

            // Register services IOC
            Infrastructure.DependencyInjection.AddInfrastructure(services);
            Application.DependencyInjection.AddApplication(services);
            Presentation.DependencyInjection.AddPresentation(services);

            // Add middleware exception handling service
            services.AddTransient<ExceptionHandling>();
            services.AddTransient<SecurityHeaders>();

            return builder;
        }

        #region COMMENTS
        //private static bool IsAuthorized(AuthorizationHandlerContext context, string neededRole, ConfigurationManager configuration)
        //{
        //    var auth = new SharedLibrary.Authorization.Authorization();

        //    var claimsClientRoles = context.User.FindFirst(c => c.Type == "resource_access");
        //    var claimsRealmRoles = context.User.FindFirst(c => c.Type == "realm_access");
        //    var clientName = configuration["Values:Keycloak.Auth.Client.Name"];
        //    var userRoles = auth.GetRoles(claimsClientRoles, claimsRealmRoles, clientName);

        //    return userRoles.Any() && userRoles.Contains(neededRole);
        //}

        /*// Add Authentication
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
        o.Authority = configuration["Values:Keycloak.Auth.Authority"];
        o.Audience = configuration["Values:Keycloak.Auth.Client.Name"];
        o.RequireHttpsMetadata = (environment.EnvironmentName != "Local"
                                  && environment.EnvironmentName != "IntegrationTests"
                                  && environment.EnvironmentName != "LocalIntegrationTests");
        o.SaveToken = true;
    });

            // Add authorisation policies
            services.AddAuthorization(options =>
            {
                options.AddPolicy("PlantOperator", policy =>
                    policy.RequireAssertion(context => IsAuthorized(context, Roles.PlantOperator.ToString(), configuration)));
                options.AddPolicy("VPPOperator", policy =>
                    policy.RequireAssertion(context => IsAuthorized(context, Roles.VPPOperator.ToString(), configuration)));

                options.AddPolicy("VPPOperatorOrBasicAccessGroup", policy =>
                    policy.RequireAssertion((context) =>
                    {
                        return IsAuthorized(context, Roles.VPPOperator.ToString(), configuration) ||
                            IsAuthorized(context, Roles.PlantOperator.ToString(), configuration) ||
                        IsAuthorized(context, Roles.BasicUser.ToString(), configuration) ||
                        IsAuthorized(context, Roles.SalesPartner.ToString(), configuration);
}));

// This policy is used in CuPoAuthentication middleware
options.AddPolicy("BasicAccessGroup", policy =>
policy.RequireAssertion(context => IsAuthorized(context, Roles.BasicUser.ToString(), configuration) || IsAuthorized(context, Roles.SalesPartner.ToString(), configuration) || IsAuthorized(context, Roles.PlantOperator.ToString(), configuration)));
options.AddPolicy("External", policy =>
policy.RequireAssertion(context => IsAuthorized(context, Roles.external.ToString(), configuration)));

options.AddPolicy("CuPoOperator", policy =>
     policy.RequireAssertion(context => IsAuthorized(context, Roles.CuPoOperator.ToString(), configuration)));

options.AddPolicy("SalesPartner", policy =>
     policy.RequireAssertion(context => IsAuthorized(context, Roles.SalesPartner.ToString(), configuration)));

options.AddPolicy("BasicUser", policy =>
     policy.RequireAssertion(context => IsAuthorized(context, Roles.BasicUser.ToString(), configuration)));
            });*/
        #endregion
    }
}
