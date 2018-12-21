using Kts.RefactorThis.Common;
using Microsoft.Extensions.Configuration;

namespace Kts.RefactorThis.Api.Config
{
    /// <summary>
    /// Provides functionality to build application defaults
    /// </summary>
    public interface IAppConfigurationFactory
    {
        AppConfiguration Build(IConfiguration configuration);
    }
}
