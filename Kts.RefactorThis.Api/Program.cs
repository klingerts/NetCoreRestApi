using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Kts.RefactorThis.Api.Config;
using Kts.RefactorThis.Common;
using Kts.RefactorThis.Setup;

namespace Kts.RefactorThis.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var builder = WebHost.CreateDefaultBuilder(args);

            // TODO: Improve configurator discovery (OCP)
            builder.ConfigureServices(services => services.AddTransient<IAppConfigurationFactory, AppConfigurationFactory>())
                   .ConfigureServices(services => services.AddTransient<IConnectionStringsFactory, ConnectionStringsFactory>())
                   .ConfigureServices(services => services.AddTransient<IExceptionHandlerOptionsFactory, ExceptionHandlerOptionsFactory>())
                   .ConfigureServices(services => services.AddTransient<IElmahConfigurator, ElmahConfigurator>())
                   .ConfigureServices(services => services.AddTransient<ISwaggerConfigurator, SwaggerConfigurator>())
                   .ConfigureServices(services => services.AddTransient<IDependencySetup, DependencySetup>())
                   .UseKestrel(options => options.AddServerHeader = false)
                   .UseStartup<Startup>();

            return builder;
        }
    }
}
