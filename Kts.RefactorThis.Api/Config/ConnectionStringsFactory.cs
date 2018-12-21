using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Kts.RefactorThis.Common;

namespace Kts.RefactorThis.Api.Config
{
    /// <summary>
    /// Provides functionality to build connection strings
    /// </summary>
    public class ConnectionStringsFactory : IConnectionStringsFactory
    {
        public ConnectionStrings Build(IConfiguration configuration,
                                       IHostingEnvironment hostingEnvironment)
        {
            var connectionStrings = configuration.GetSection(nameof(ConnectionStrings))
                                                 .Get<ConnectionStrings>();

            if (hostingEnvironment.IsDevelopment())
            {
                connectionStrings.Default = connectionStrings.Default
                                                             .Replace("|DataDirectory|", hostingEnvironment.ContentRootPath);
            }

            return connectionStrings;
        }
    }
}
