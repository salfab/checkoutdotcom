using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using checkoutdotcom.Controllers;
using checkoutdotcom.Filters;
using checkoutdotcom.Middlewares;

namespace checkoutdotcom
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Build the policies that will be used to grand access, or returned an Unauthorized Http status code
            services.AddAuthorization(
                options =>
                    {
                        // Policies can be extended with authorization handlers and Authorization Requirements if the logic needs to be more complex.
                        // At the moment, it just verifies the presence of an API key (as a required claim).
                        options.AddPolicy("ValidApiKey",policy => policy.RequireClaim("ApiKeyIdentity"));
                    });

            services.AddMvc(
                options =>
                    {
                        // register our filters, globally.
                        options.Filters.Add(typeof(ModelStateValidationActionFilter));
                        options.Filters.Add(typeof(ResourceNotFoundExceptionToHttpStatusCodeConverterActionFilter));
                        options.Filters.Add(typeof(ArgumentExceptionToBadRequestActionFilter));
                    });

            RegisterServices(services);
        }

        private static void RegisterServices(IServiceCollection services)
        {
            // Register types in IoC
            services.AddSingleton<IDrinksCountTrackingService, DrinksCountTrackingService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(this.Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // Register the middleware that is going to build the identity containing the API Key claim.
            app.UseMiddleware<AuthorizationMiddleware>();
            app.UseMvc();
        }
    }
}
