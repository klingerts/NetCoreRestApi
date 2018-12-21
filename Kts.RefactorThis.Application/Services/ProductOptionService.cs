using System;
using System.Threading.Tasks;
using FluentValidation;
using Kts.RefactorThis.Application.Abstractions;
using Kts.RefactorThis.Common;
using Kts.RefactorThis.Common.DependencyMarkers;
using Kts.RefactorThis.DTO;

namespace Kts.RefactorThis.Application.Services
{
    public class ProductOptionService : ServiceBase, IProductOptionService, IPerRequestDependency
    {
        protected readonly IUnitOfWorkFactory _uowFactory;
        protected readonly IProductOptionRepository _productOptionRepo;
        protected readonly IValidator<CreateProductOptionDTO> _createValidator;
        protected readonly IValidator<UpdateProductOptionDTO> _updateValidator;

        public ProductOptionService(IUnitOfWorkFactory uowFactory, 
                                    IProductOptionRepository productOptionRepo, 
                                    IValidator<CreateProductOptionDTO> createValidator,
                                    IValidator<UpdateProductOptionDTO> updateValidator)
        {
            _uowFactory = uowFactory;
            _productOptionRepo = productOptionRepo;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public virtual async Task<OperationResult<Guid>> CreateAsync(CreateProductOptionDTO optionToCreate)
        {
            var validationResult = _createValidator.Validate(optionToCreate);
            if (!validationResult.IsValid) return Fail<Guid>(validationResult);

            Guid productId = await _productOptionRepo.CreateAsync(optionToCreate);

            return Success(productId);
        }

        public virtual async Task<OperationResult<bool>> UpdateAsync(UpdateProductOptionDTO optionToUpdate)
        {
            var validationResult = _updateValidator.Validate(optionToUpdate);
            if (!validationResult.IsValid) return Fail<bool>(validationResult);

            var updated = await _productOptionRepo.UpdateAsync(optionToUpdate);

            return Success(updated);
        }

        public virtual async Task<OperationResult<bool>> DeleteAsync(Guid productId, Guid optionOptionId)
        {
            bool deleted = await _productOptionRepo.DeleteAsync(productId, optionOptionId);
            return Success(deleted);
        }
    }
}
