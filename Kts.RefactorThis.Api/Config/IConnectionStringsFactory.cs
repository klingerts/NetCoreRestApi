using Kts.RefactorThis.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Kts.RefactorThis.Api.Config
{
    /// <summary>
    /// Provides functionality to build connection strings
    /// </summary>
    public interface IConnectionStringsFactory
    {
        ConnectionStrings Build(IConfiguration configuration,
                                IHostingEnvironment hostingEnvironment);
    }
}
