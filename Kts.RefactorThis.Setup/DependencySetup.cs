using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using FluentValidation;
using Kts.RefactorThis.Application;
using Kts.RefactorThis.DataAccess;
using Kts.RefactorThis.Common.DependencyMarkers;
using Kts.RefactorThis.Common;

namespace Kts.RefactorThis.Setup
{
    /// <summary>
    /// Register application dependencies in DI container
    /// </summary>
    /// <Remarks>
    /// In addition to the assemblies received as argument, the scanning process also
    /// scans assemblies containing the declaration of IApplicationMarker, IDataAccessMarker, ICommonMarker.
    /// The scanning process looks for classes that implement IPerInstanceDependency and IPerRequestDependency.
    /// AutoMapper profile and FluentValidation validation classes are also registered.
    /// </Remarks>

    public class DependencySetup : IDependencySetup
    {
        /// <exception cref="ArgumentNullException">When any of the following is null: 
        /// services, appConfiguration, connectionStrings, startupAssemblies
        /// </exception>
        public virtual void RegisterAllDependencies(IServiceCollection services, 
                                            AppConfiguration appConfiguration,
                                            ConnectionStrings connectionStrings,
                                            IEnumerable<Assembly> startupAssemblies)

        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (appConfiguration == null) throw new ArgumentNullException(nameof(appConfiguration));
            if (connectionStrings == null) throw new ArgumentNullException(nameof(connectionStrings));
            if (startupAssemblies == null) throw new ArgumentNullException(nameof(startupAssemblies));

            services.AddSingleton(appConfiguration);
            services.AddSingleton(connectionStrings);

            var assemblies = new List<Assembly>(startupAssemblies);
            assemblies.AddRange(new Assembly[] { typeof(IApplicationMarker).Assembly,
                                                 typeof(IDataAccessMarker).Assembly,
                                                 typeof(ICommonMarker).Assembly});

            RegisterMappingProfiles(services, assemblies);
            RegisterApplicationDependencies(services, assemblies);
            RegisterFluentValidationValidations(services, assemblies); 
        }

        // Registers automapper profile classes to container
        protected virtual void RegisterMappingProfiles(IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            services.AddAutoMapper(assemblies);
        }

        // Registers classes marked with IPerInstanceDependency and IPerRequestDependency to container
        protected virtual void RegisterApplicationDependencies(IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            services.Scan(scan => scan.FromAssemblies(assemblies)
                    .AddClasses(filter => filter.IsAssignableTo<IPerInstanceDependency>())
                    .AsImplementedInterfaces().WithTransientLifetime()
                    .AddClasses(filter => filter.IsAssignableTo<IPerRequestDependency>())
                    .AsImplementedInterfaces().WithScopedLifetime()
                    // Add classes without interfaces (other than the markers)
                    .AddClasses(filter => filter.IsSelfOnly<IPerInstanceDependency>())
                    .AsSelf().WithTransientLifetime()
                    .AddClasses(filter => filter.IsSelfOnly<IPerRequestDependency>())
                    .AsSelf().WithScopedLifetime()
                    );
        }

        // Registers validation classes to container
        protected virtual void RegisterFluentValidationValidations(IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            services.Scan(scan => scan.FromAssemblies(assemblies)
                                      .AddClasses(filter => filter.Where(c => c.IsAssignableToType<IValidator>()))
                                      .AsImplementedInterfaces().WithTransientLifetime());
        }
    }
}
