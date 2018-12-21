using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Kts.RefactorThis.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using ElmahCore;
using ElmahCore.Mvc;

namespace Kts.RefactorThis.Api.Config
{
    /// <summary>
    /// Provides functionality to configure Elmah    
    /// </summary>
    public class ElmahConfigurator : IElmahConfigurator
    {
        //TODO: Improve elmah configuration
        public void ConfigureServices(IServiceCollection services,
                                      IConfiguration configuration,
                                      IHostingEnvironment hostingEnvironment)
        {
            var elmahConfig = configuration.GetSection("ElmahCore")
                                    .Get<ElmahConfig>();

            services.AddElmah<XmlFileErrorLog>(options =>
            {
                options.Path = elmahConfig.Path;
                options.LogPath = elmahConfig.LogPath;
                if (!hostingEnvironment.IsDevelopment())
                {
                    options.CheckPermissionAction = context => context.User.Identity.IsAuthenticated;
                }
            });
        }

        public void ConfigureMiddleware(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseElmah();
        }
    }
}
