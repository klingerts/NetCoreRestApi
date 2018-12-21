using Kts.RefactorThis.DTO;
using Kts.RefactorThis.Common;
using System;
using System.Threading.Tasks;
using FluentValidation;
using Kts.RefactorThis.Application.Abstractions;
using Kts.RefactorThis.Common.DependencyMarkers;

namespace Kts.RefactorThis.Application.Services
{
    public class ProductService : ServiceBase, IProductService, IPerRequestDependency
    {
        protected readonly IUnitOfWorkFactory _uowFactory;
        protected readonly IProductRepository _productRepo;
        protected readonly IValidator<CreateProductDTO> _createValidator;
        protected readonly IValidator<UpdateProductDTO> _updateValidator;

        public ProductService(IUnitOfWorkFactory uowFactory, 
                              IProductRepository productRepo, 
                              IValidator<CreateProductDTO> createValidator,
                              IValidator<UpdateProductDTO> updateValidator)
        {
            _uowFactory = uowFactory;
            _productRepo = productRepo;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public virtual async Task<OperationResult<Guid>> CreateAsync(CreateProductDTO productToCreate)
        {
            var validationResult = _createValidator.Validate(productToCreate);
            if (!validationResult.IsValid) return Fail<Guid>(validationResult);

            Guid productId = await _productRepo.CreateAsync(productToCreate);

            return Success(productId);
        }

        public virtual async Task<OperationResult<bool>> UpdateAsync(UpdateProductDTO productToUpdate)
        {
            var validationResult = _updateValidator.Validate(productToUpdate);
            if (!validationResult.IsValid) return Fail<bool>(validationResult);

            var updated = await _productRepo.UpdateAsync(productToUpdate);

            return Success(updated);
        }

        public virtual async Task<OperationResult<bool>> DeleteAsync(Guid id)
        {
            bool deleted = false;
            using (var unitOfWork = _uowFactory.GetNewUnitOfWork())
            {
                unitOfWork.BeginTransaction();
                deleted = await _productRepo.DeleteAsync(id, unitOfWork);
                unitOfWork.Commit();
            }
            return Success(deleted);
        }
    }
}
