using FluentValidation;

namespace Kts.RefactorThis.Common
{
    /// <summary>
    /// Pagination parameters
    /// </summary>
    public class PaginationParams
    {
        public int Limit { get; set; }
        public int Offset { get; set; }
    }

    public class PaginationParamsValidator : AbstractValidator<PaginationParams>
    {
        public PaginationParamsValidator(AppConfiguration appDefaults)
        {
            RuleFor(o => o.Offset).GreaterThan(-1);
            RuleFor(o => o.Limit).InclusiveBetween(0, appDefaults.QueryRowsLimit)
                                 .LessThanOrEqualTo(AppConfiguration.MaxQueryRowsLimitDefault);
        }
    }
}
