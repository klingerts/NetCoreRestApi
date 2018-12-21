using FluentValidation.Results;

namespace Kts.RefactorThis.Common
{
    /// <summary>
    /// Represents the result of an operation
    /// </summary>
    /// <typeparam name="TReturnValue">Data to be returned</typeparam>
    public class OperationResult<TReturnValue>: OperationResult
    {       
        public OperationResult(bool success, TReturnValue data = default(TReturnValue), 
                               ValidationResult validationResult = null, string message = null) 
             : base(success, validationResult, message)
        {
            Data = data;
        }

        public TReturnValue Data { get; set; }
    }

    /// <summary>
    /// Represents the result of an operation
    /// </summary>
    public class OperationResult
    {
        private ValidationResult _validationResult;

        public OperationResult(bool success, ValidationResult validationResult = null, string message = null)
        {
            _validationResult = validationResult ?? new ValidationResult();
            Success = success;
            Message = message;
        }

        public ValidationResult GetValidationResult()
        {
            return _validationResult;
        }

        public bool HasValidationErrors
        {
            get { return _validationResult != null && !_validationResult.IsValid; }
        }

        public bool Success { get; private set; }
        public string Message { get; private set; }
        
        public override string ToString()
        {
            return $"Success: {Success} HasValidationErrors: {HasValidationErrors} - ValidationErrorCount: {_validationResult?.Errors?.Count}";
        }
    }
}