using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Kts.RefactorThis.Common;

namespace Kts.RefactorThis.Setup
{
    /// <summary>
    /// Provides functionality to register dependencies to DI container
    /// </summary>
    public interface IDependencySetup
    {
        void RegisterAllDependencies(IServiceCollection services,
                                     AppConfiguration appDefaults,
                                     ConnectionStrings connectionStrings,
                                     IEnumerable<Assembly> startupAssemblies);
    }
}