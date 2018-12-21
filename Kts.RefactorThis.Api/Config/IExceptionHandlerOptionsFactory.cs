using Kts.RefactorThis.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Kts.RefactorThis.Api.Config
{
    public interface IExceptionHandlerOptionsFactory
    {
        ExceptionHandlerOptions Build(AppConfiguration appConfiguration, IHostingEnvironment hostingEnvironment);
    }
}
