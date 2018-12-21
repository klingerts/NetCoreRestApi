using FluentValidation;
using Kts.RefactorThis.DTO;
using System;

namespace Kts.RefactorThis.Application.Validators
{
    public class CreateProductOptionValidator<T> : AbstractValidator<T> where T : CreateProductOptionDTO
    {
        public CreateProductOptionValidator() : base()
        {
            RuleFor(o => o.ProductId).NotEqual(Guid.Empty);
            RuleFor(o => o.Name).NotNull().Length(1, 100).IsSafeString();
            RuleFor(o => o.Description).Length(0, 500).IsSafeString();
        }
    }

    public class CreateProductOptionValidator : CreateProductOptionValidator<CreateProductOptionDTO>
    {
    }
}
