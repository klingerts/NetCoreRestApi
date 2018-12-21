using System;
using System.Threading.Tasks;
using Kts.RefactorThis.Common;
using Kts.RefactorThis.DTO;

namespace Kts.RefactorThis.Application.Services
{
    public interface IProductOptionService
    {
        Task<OperationResult<Guid>> CreateAsync(CreateProductOptionDTO optionToCreate);
        Task<OperationResult<bool>> DeleteAsync(Guid productId, Guid optionId);
        Task<OperationResult<bool>> UpdateAsync(UpdateProductOptionDTO optionToUpdate);
    }
}