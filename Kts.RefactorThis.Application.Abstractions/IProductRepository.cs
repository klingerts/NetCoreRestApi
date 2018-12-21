using System;
using System.Threading.Tasks;
using Kts.RefactorThis.DTO;

namespace Kts.RefactorThis.Application.Abstractions
{
    /// <summary>
    /// Used to create/update/delete records in Product table
    /// Unit of work is an optional parameter.
    /// </summary>
    public interface IProductRepository
    {
        // Create new row and returns id created
        // TODO: Make this generic so that it can support any key
        Task<Guid> CreateAsync(CreateProductDTO product, IUnitOfWork unitOfWork = null);
        
        // Updates product and returns true when row updated, false if no row updated
        Task<bool> UpdateAsync(UpdateProductDTO product, IUnitOfWork unitOfWork = null);
        
        // Delete product and returns true when row deleted, false if no row deleted
        Task<bool> DeleteAsync(Guid productId, IUnitOfWork unitOfWork = null);
    }
}
