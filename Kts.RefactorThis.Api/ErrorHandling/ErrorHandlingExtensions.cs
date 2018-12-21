using FluentValidation.AspNetCore;
using Kts.RefactorThis.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Kts.RefactorThis.Api.ErrorHandling
{
    public static class ErrorHandlingExtensions
    {
        /// <summary>
        /// Load OperationResult validation errors in modelstate and return 
        /// instance of ProblemDetail with validation errors.
        /// </summary>
        /// <param name="modelState">Current model state</param>
        /// <param name="result">an OperationResult</param>
        /// <param name="prefix">optional prefix ro be added to modelstate errors</param>
        /// <returns>Instance of ProblemDetailsWithValidation</returns>
        public static ProblemDetailsWithValidation AsProblemDetail(this ModelStateDictionary modelState, 
                                                                   OperationResult result = null,                                                                    
                                                                   string prefix = null )
        {
            result?.GetValidationResult()?.AddToModelState(modelState, prefix);

            return new ProblemDetailsWithValidation(new SerializableError(modelState));
        }
    }
}
