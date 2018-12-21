using System;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Kts.RefactorThis.Api.Config;
using Kts.RefactorThis.Setup;
using Kts.RefactorThis.Common;

namespace Kts.RefactorThis.Api.Integration.Tests
{
    public class TestWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected string _environment;
        public TestWebApplicationFactory(string environment) : base()
        {
            _environment = environment;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // TODO: Improve this

            if (string.IsNullOrEmpty(_environment))
            {
                var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                _environment = environmentName ?? "Development";
            }
            builder.UseEnvironment(_environment);

            base.ConfigureWebHost(builder);
            builder.ConfigureServices(services => services.AddTransient<IAppConfigurationFactory, AppConfigurationFactory>())
                   .ConfigureServices(services => services.AddTransient<IConnectionStringsFactory, ConnectionStringsFactory>())
                   .ConfigureServices(services => services.AddTransient<IExceptionHandlerOptionsFactory, ExceptionHandlerOptionsFactory>())
                   .ConfigureServices(services => services.AddTransient<IElmahConfigurator, ElmahConfigurator>())
                   .ConfigureServices(services => services.AddTransient<ISwaggerConfigurator, SwaggerConfigurator>())
                   .ConfigureServices(services => services.AddTransient<IDependencySetup, DependencySetup>())
                   .UseKestrel(options => options.AddServerHeader = false)
                   .UseStartup<Startup>();


        }
    }
}
