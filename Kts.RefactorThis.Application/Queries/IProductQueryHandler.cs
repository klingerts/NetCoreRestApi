using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kts.RefactorThis.DTO;
using Kts.RefactorThis.Common;

namespace Kts.RefactorThis.Application.Queries
{
    /// <summary>
    /// Provides functionality to query products
    /// </summary>
    public interface IProductQueryHandler
    {
        // Get product by id
        Task<QueryProductDTO> GetProductByIdAsync(Guid id);
        // Get products
        Task<IEnumerable<QueryProductDTO>> GetProductsAsync(PaginationParams pagination, string name = null);
    }
}
