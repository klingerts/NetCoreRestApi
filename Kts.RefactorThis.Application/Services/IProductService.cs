using System;
using System.Threading.Tasks;
using Kts.RefactorThis.Common;
using Kts.RefactorThis.DTO;

namespace Kts.RefactorThis.Application.Services
{
    public interface IProductService
    {
        Task<OperationResult<Guid>> CreateAsync(CreateProductDTO productToCreate);
        Task<OperationResult<bool>> DeleteAsync(Guid id);
        Task<OperationResult<bool>> UpdateAsync(UpdateProductDTO productToUpdate);
    }
}