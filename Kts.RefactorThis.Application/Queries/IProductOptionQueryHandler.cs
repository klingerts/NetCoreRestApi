using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kts.RefactorThis.DTO;
using Kts.RefactorThis.Common;

namespace Kts.RefactorThis.Application.Queries
{
    /// <summary>
    /// Provides functionality to query productOption
    /// </summary>
    public interface IProductOptionQueryHandler
    {
        Task<QueryProductOptionDTO> GetProductOptionAsync(Guid productId, Guid optionId);
        Task<IEnumerable<QueryProductOptionDTO>> GetProductOptionsByProductIdAsync(PaginationParams pagination, Guid productId);
    }
}
