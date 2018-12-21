using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kts.RefactorThis.Common
{
    /// <summary>
    /// Provides functionality to configure Swagger
    /// </summary>
    public interface ISwaggerConfigurator
    {
        void ConfigureServices(IServiceCollection services,
                               AppConfiguration appConfiguration,
                               IConfiguration configuration,
                               IHostingEnvironment hostingEnvironment);

        void ConfigureMiddleware(IApplicationBuilder app, 
                                 IHostingEnvironment env);
    }
}
