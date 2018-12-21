using FluentValidation;
using Kts.RefactorThis.DTO;

namespace Kts.RefactorThis.Application.Validators
{
    public class CreateProductValidator<T> : AbstractValidator<T> where T : CreateProductDTO
    {
        public CreateProductValidator()
        {
            RuleFor(o => o.Name).NotNull().Length(1, 100).IsSafeString(); ;
            RuleFor(o => o.Description).Length(0, 500).IsSafeString(); ;
            RuleFor(o => o.Price).GreaterThan(0);
            RuleFor(o => o.DeliveryPrice).GreaterThanOrEqualTo(0);
        }
    }

    public class CreateProductValidator : CreateProductValidator<CreateProductDTO>
    {
    }

}
