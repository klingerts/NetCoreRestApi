using Microsoft.Extensions.Configuration;
using Kts.RefactorThis.Common;

namespace Kts.RefactorThis.Api.Config
{
    /// <summary>
    /// Provides functionality to build application defaults
    /// </summary>
    public class AppConfigurationFactory : IAppConfigurationFactory
    {
        public AppConfiguration Build(IConfiguration configuration)
        {
            var appConfiguration = configuration.GetSection(nameof(AppConfiguration))
                                                .Get<AppConfiguration>();

            return appConfiguration;
        }
    }
}
