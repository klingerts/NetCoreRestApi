using FluentValidation;
using Kts.RefactorThis.DTO;
using System;

namespace Kts.RefactorThis.Application.Validators
{
    public class UpdateProductValidator : CreateProductValidator<UpdateProductDTO>
    {
        public UpdateProductValidator() : base()
        {
            RuleFor(o => o.Id).NotEqual(Guid.Empty);
        }
    }
}
