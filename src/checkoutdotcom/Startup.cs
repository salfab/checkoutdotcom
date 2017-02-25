using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using checkoutdotcom.Controllers;
using checkoutdotcom.Filters;

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
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(
                options =>
                    {
                        // register our filters globally.
                        options.Filters.Add(typeof(ModelStateValidationActionFilter));
                        options.Filters.Add(typeof(ResourceNotFoundExceptionToHttpStatusCodeConverterActionFilter));
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
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}
