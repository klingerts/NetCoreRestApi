using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Kts.RefactorThis.Common;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;
using System.IO;
using System;

namespace Kts.RefactorThis.Api.Config
{
    /// <summary>
    /// Provides functionality to configure Swagger
    /// </summary>
    public class SwaggerConfigurator : ISwaggerConfigurator
    {
        protected Info _apiInfo;
        protected string _swaggerEndpoint;

        //TODO: Improve Swagger configuration
        public void ConfigureServices(IServiceCollection services,
                                      AppConfiguration appConfiguration,
                                      IConfiguration configuration,
                                      IHostingEnvironment hostingEnvironment)
        {
            _apiInfo = configuration.GetSection("SwaggerInfo")
                                    .Get<Info>();

            _swaggerEndpoint = appConfiguration.SwaggerEndpoint.Replace("{Version}", _apiInfo.Version);

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(_apiInfo.Version, _apiInfo);
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.{_apiInfo.Version}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        public void ConfigureMiddleware(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(_swaggerEndpoint, _apiInfo.Title);

                if (env.IsProduction())
                {
                    c.SupportedSubmitMethods(new[] { SubmitMethod.Get, SubmitMethod.Head });
                }
                else
                {
                    c.SupportedSubmitMethods(new[] {
                    SubmitMethod.Get, SubmitMethod.Head,
                    SubmitMethod.Post, SubmitMethod.Delete,
                    SubmitMethod.Put });
                }               
            });
        }
    }
}
