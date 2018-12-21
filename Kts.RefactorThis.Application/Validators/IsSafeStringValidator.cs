using FluentValidation;
using FluentValidation.Validators;
using Ganss.XSS;

namespace Kts.RefactorThis.Application.Validators
{
    /// <summary>
    /// Custom validation used to detect strings containing potential harmful data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class IsSafeStringValidator<T> : PropertyValidator
    {
        private static IHtmlSanitizer htmlSanitizer = new HtmlSanitizer();

        public IsSafeStringValidator()
            : base("{PropertyName} must not contain unsafe content.")
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var str = context.PropertyValue as string;
            if (str == null) return true;

            var sanitized = htmlSanitizer.Sanitize(str);
            if (sanitized != str) return false;

            return true;
        }
    }

    /// <summary>
    /// Extension method for validator.
    /// Usage: RuleFor<>.IsSafeString()
    /// </summary>
    public static class RefactorThisValidatorExtensions
    {
        public static IRuleBuilderOptions<T, TElement> IsSafeString<T, TElement>(this IRuleBuilder<T, TElement> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new IsSafeStringValidator<TElement>());
        }
    }
}
