using Kts.RefactorThis.Common;
using FluentValidation.Results;
using Kts.RefactorThis.Common.DependencyMarkers;

namespace Kts.RefactorThis.Application.Abstractions
{
    /// <summary>
    /// Helper methods to create OperationResults
    /// </summary>
    public abstract class ServiceBase
    {
        public ServiceBase()
        {
        }

        protected OperationResult<TReturnValue> Success<TReturnValue>(TReturnValue returnValue, string message = null)
        {
            var cr = new OperationResult<TReturnValue>(true, returnValue, null, message);
            return cr;
        }

        protected OperationResult<TReturnValue> Fail<TReturnValue>(ValidationResult validationResult, string message)
        {
            var cr = new OperationResult<TReturnValue>(false, default(TReturnValue), validationResult, message);
            return cr;
        }

         protected OperationResult<TReturnValue> Fail<TReturnValue>(ValidationResult validationResult)
        {
            return Fail<TReturnValue>(validationResult, null);
        }

        protected OperationResult<TReturnValue> Fail<TReturnValue>(string message)
        {
            return Fail<TReturnValue>(null, message);
        }

        protected OperationResult Success(string message = null)
        {
            var cr = new OperationResult(true, null, message);
            return cr;
        }

        protected OperationResult Fail(ValidationResult validationResult, string message)
        {
            var cr = new OperationResult(false, validationResult, message);
            return cr;
        }

        protected OperationResult Fail(ValidationResult validationResult)
        {
            return Fail(validationResult, null);
        }

        protected OperationResult Fail(string message)
        {
            return Fail(null, message);
        }

    }
}
