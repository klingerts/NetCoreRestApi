using FluentValidation;
using Kts.RefactorThis.DTO;
using System;

namespace Kts.RefactorThis.Application.Validators
{
    public class UpdateProductOptionValidator : CreateProductOptionValidator<UpdateProductOptionDTO>
    {
        public UpdateProductOptionValidator() : base()
        {
            RuleFor(o => o.OptionId).NotEqual(Guid.Empty);
        }
    }
}
