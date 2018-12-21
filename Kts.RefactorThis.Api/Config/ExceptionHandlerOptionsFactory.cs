using Kts.RefactorThis.Api.ErrorHandling;
using Kts.RefactorThis.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;

namespace Kts.RefactorThis.Api.Config
{
    public class ExceptionHandlerOptionsFactory : IExceptionHandlerOptionsFactory
    {
        public ExceptionHandlerOptions Build(AppConfiguration appConfiguration, IHostingEnvironment hostingEnvironment)
        {
            bool isDevelopment = hostingEnvironment.IsDevelopment();
            bool readableJson = appConfiguration.ReturnsHumanReadableJson;

            var options = new ExceptionHandlerOptions()
            {
                ExceptionHandler = async (context) =>
                {
                    CustomProblemDetails problemDetail = null;

                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var exception = contextFeature.Error;
                    var badRequest = exception as BadHttpRequestException;

                    if (badRequest == null)
                    {
                        // HTTP 500
                        // TODO: Detect app safe exceptions so that message can be sent back
                        string detail = isDevelopment
                            ? exception.ToStringDemystified()
                            : "Use Instance to identify issue";

                        problemDetail = new CustomProblemDetails("error")
                        {
                            Title = "Internal Server Error",
                            Status = context.Response.StatusCode,
                            Detail = detail
                        };
                    }
                    else
                    {
                        // Bad request
                        // TODO: Recover original status code
                        int statusCode = (int)HttpStatusCode.BadRequest;
                        string title = badRequest.Message;
                        problemDetail = new CustomProblemDetails("badrequest")
                        {
                            Title = badRequest.Message,
                            Status = statusCode,
                            Detail = "Use Instance value to identify the issue"
                        };
                    }

                    var jsonFormat = readableJson ? Formatting.Indented : Formatting.None;
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(problemDetail, jsonFormat));
                }
            };

            return options;
        }
    }
}
