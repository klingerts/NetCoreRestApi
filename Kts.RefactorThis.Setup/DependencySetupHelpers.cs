using System;
using Scrutor;
using Kts.RefactorThis.Common.DependencyMarkers;

namespace Kts.RefactorThis.Setup
{
    public static class DependencySetupHelpers
    { 
        //
        // Filters types that inherit to non marker interfaces
        public static IImplementationTypeFilter IsAssignableTo<T>(this IImplementationTypeFilter filter)
        {
            return filter.Where(c => c.IsAssignableToType<T>() && !c.IsAssignableToType<IRegisterAsSelf>());
        }

        // Filters types that inherit from marker interface only
        public static IImplementationTypeFilter IsSelfOnly<T>(this IImplementationTypeFilter filter)
        {
            return filter.Where(c => c.IsAssignableToType<T>() && c.IsAssignableToType<IRegisterAsSelf>());
        }

        // Check if type implements type T and at least one more type
        public static bool IsAssignableToType<T>(this Type type)
        {
            bool isAssignable = typeof(T).IsAssignableFrom(type) 
                                && !type.IsGenericTypeDefinition;

            return isAssignable;
        }
    }
}
