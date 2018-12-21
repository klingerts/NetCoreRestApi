using System;
using System.Threading.Tasks;
using Kts.RefactorThis.DTO;

namespace Kts.RefactorThis.Application.Abstractions
{
    /// <summary>
    /// Used to read (by id)/create/update/delete records in ProductOption table
    /// Unit of work is an optional parameter.
    /// </summary>
    public interface IProductOptionRepository
    {
        // Get optionProduct by id
        Task<QueryProductOptionDTO> GetProductOptionByIdAsync(Guid productId, Guid optionId);

        // Create optionProduct row and returns id for new product
        Task<Guid> CreateAsync(CreateProductOptionDTO product, IUnitOfWork unitOfWork = null);
        
        // Update optionProduct row and returns true when row updated, false if no row updated
        Task<bool> UpdateAsync(UpdateProductOptionDTO product, IUnitOfWork unitOfWork = null);

        // Delete optionProduct row and returns true when row deleted, false if no row deleted
        Task<bool> DeleteAsync(Guid productId, Guid productOptionId, IUnitOfWork unitOfWork = null);       
    }
}
