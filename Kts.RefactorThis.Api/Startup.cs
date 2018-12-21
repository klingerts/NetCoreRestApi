using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using FluentValidation.AspNetCore;
using Kts.RefactorThis.Common;
using Kts.RefactorThis.Setup;
using Kts.RefactorThis.Api.Config;
using Kts.RefactorThis.Api.ErrorHandling;

namespace Kts.RefactorThis.Api
{
    public class Startup
    {
        protected readonly IConfiguration _configuration;
        protected readonly IHostingEnvironment _hostingEnvironment;
        protected readonly AppConfiguration _appConfiguration;
        protected readonly ConnectionStrings _connectionStrings;
        protected readonly IElmahConfigurator _elmahConfigurator;
        protected readonly ISwaggerConfigurator _swaggerConfigurator;
        protected readonly IDependencySetup _dependencySetup;
        protected readonly IExceptionHandlerOptionsFactory _exceptionHandlerOptionsFactory;

        public Startup(IConfiguration configuration,
                       IHostingEnvironment hostingEnvironment,
                       IAppConfigurationFactory appConfigurationFactory,
                       IConnectionStringsFactory connectionStringsFactory,
                       IDependencySetup dependencySetup,
                       IExceptionHandlerOptionsFactory exceptionHandlerOptionsFactory,
                       IElmahConfigurator elmahConfigurator,
                       ISwaggerConfigurator swaggerConfigurator)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            _dependencySetup = dependencySetup;
            _exceptionHandlerOptionsFactory = exceptionHandlerOptionsFactory;

            _elmahConfigurator = elmahConfigurator;
            _swaggerConfigurator = swaggerConfigurator;

             _appConfiguration = appConfigurationFactory.Build(_configuration);
             _connectionStrings = connectionStringsFactory.Build(_configuration, _hostingEnvironment);
        }

        // Use this method to add services to the container.
        // This method gets called by the runtime. 
        public void ConfigureServices(IServiceCollection services)
        {
            // TODO: Configure log
            // TODO: Content negotiation
            _dependencySetup.RegisterAllDependencies(services,
                                                     _appConfiguration, _connectionStrings,
                                                     new[] { typeof(Startup).Assembly });

            services.AddMvcCore()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                    .AddApiExplorer()
                    .AddJsonFormatters()
                    .AddJsonOptions(options =>
                    {
                        // Preserves case on JSON conversion
                        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                        if (_appConfiguration.ReturnsHumanReadableJson)
                        {
                            options.SerializerSettings.Formatting = Formatting.Indented;
                        }
                        options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
                    })
                    // TODO: .AddCors() ?
                    .AddDataAnnotations()
                    .AddFluentValidation();

            services.Configure((Action<ApiBehaviorOptions>)(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    return new BadRequestObjectResult(context.ModelState.AsProblemDetail());
                };
            }));          

            // TODO: Improve configurator execution (OCP)
            // Add Swagger services
            if (_appConfiguration.EnableSwagger)
            {
                _swaggerConfigurator?.ConfigureServices(services, _appConfiguration,
                                                        _configuration, _hostingEnvironment);
            }

            // Add Elmah
            if (_appConfiguration.EnableElmah)
            {
                _elmahConfigurator?.ConfigureServices(services,
                                                      _configuration, _hostingEnvironment);
            }
        }

        // Use this method to configure the HTTP request pipeline. 
        // This method gets called by the runtime. 
        public void Configure(IApplicationBuilder appBuilder, IHostingEnvironment hostingEnvironment)
        {
            ConfigureExceptionHandling(appBuilder, hostingEnvironment);

            if (!hostingEnvironment.IsDevelopment())
            {
                if (!_appConfiguration.DisableHsts) appBuilder.UseHsts();
                // API Clients may not respect the require HTTPS and send data in the open
                // The only safe way is to only accept requests over HTTPS
                if (!_appConfiguration.AcceptHttp) AbortRequestsOverHTTP(appBuilder);
            }
            
            // Enable middleware to serve generated Swagger as a JSON endpoint
            if (_appConfiguration.EnableSwagger)
            {
                _swaggerConfigurator?.ConfigureMiddleware(appBuilder, hostingEnvironment);
            }

            if (_appConfiguration.EnableElmah)
            {
                _elmahConfigurator?.ConfigureMiddleware(appBuilder, hostingEnvironment);
            }

            if (!_appConfiguration.DisableHttpsRedirection)
            {
                appBuilder.UseHttpsRedirection();
            }

            appBuilder.UseMvc();
        }

        private static void AbortRequestsOverHTTP(IApplicationBuilder appBuilder)
        {
            appBuilder.Use(async (context, next) =>
            {
                if (!context.Request.IsHttps) context.Abort();
                await next();
            });
        }

        private void ConfigureExceptionHandling(IApplicationBuilder appBuilder, IHostingEnvironment hostingEnvironment)
        {
            if (hostingEnvironment.IsDevelopment())
            {
                if (_appConfiguration.UseDevPageInDevelopment)
                {
                    appBuilder.UseDeveloperExceptionPage();
                }
                else
                {
                    appBuilder.UseExceptionHandler(_exceptionHandlerOptionsFactory.Build(_appConfiguration, hostingEnvironment));
                }
            }
            else
            {
                appBuilder.UseExceptionHandler(_exceptionHandlerOptionsFactory.Build(_appConfiguration, hostingEnvironment));               
            }
        }
    }
}
