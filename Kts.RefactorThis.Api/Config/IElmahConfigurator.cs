using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kts.RefactorThis.Common
{
    /// <summary>
    /// Provides functionality to build application defaults
    /// </summary>
    public interface IElmahConfigurator
    {
        void ConfigureServices(IServiceCollection services,
                               IConfiguration configuration,
                               IHostingEnvironment hostingEnvironment);

        void ConfigureMiddleware(IApplicationBuilder app, 
                                 IHostingEnvironment env);
    }
}
