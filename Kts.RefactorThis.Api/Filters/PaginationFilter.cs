using Microsoft.AspNetCore.Mvc.Filters;
using Kts.RefactorThis.Common;
using Kts.RefactorThis.Common.DependencyMarkers;

namespace Kts.RefactorThis.Api.Filters
{
    /// <summary>
    /// Custom action filter that sets pagination defaults when limit is not provided
    /// </summary>
    public class PaginationFilter : IActionFilter, IPerInstanceDependency, IRegisterAsSelf
    {
        private readonly int _queryRowLimits;

        public PaginationFilter(AppConfiguration appConfiguration)
        {
            _queryRowLimits = appConfiguration.QueryRowsLimit;
        }
        
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Returms if model is not valid or request is not GET
            if (!context.ModelState.IsValid) return;

            // Looks for Pagination instance in action arguments
            foreach (var m in context.ActionArguments)
            {
                var pagination = m.Value as PaginationParams;
                if (pagination != null && pagination.Limit == 0)
                {
                    // Sets default value when limit is not provided
                    pagination.Limit = _queryRowLimits;
                    return;
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
