using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Kts.RefactorThis.Application.Queries;
using Kts.RefactorThis.Common;
using Kts.RefactorThis.Common.DependencyMarkers;
using Kts.RefactorThis.DTO;

namespace Kts.RefactorThis.DataAccess.QueryHandlers
{
    /// <summary>
    /// Provides functionality to query productOption records
    /// </summary>
    public class ProductOptionQueryHandler : IProductOptionQueryHandler, IPerRequestDependency
    {
        private readonly IConnectionProxy _connectionProxy;
        private readonly IProductQueryHandler _productQueryHandler;

        public ProductOptionQueryHandler(IConnectionProxy connectionProxy, IProductQueryHandler productQueryHandler)
        {
            _connectionProxy = connectionProxy;
            _productQueryHandler = productQueryHandler;
        }

        public virtual async Task<QueryProductOptionDTO> GetProductOptionAsync(Guid productId, Guid optionId)
        {
            var option = await _connectionProxy.QueryFirstOrDefaultAsync<QueryProductOptionDTO>(SQLStatements.GetProductOptionByIdAndProductId,
                         new { ProductId = productId, OptionId = optionId });

            return option;
        }

        public virtual async Task<IEnumerable<QueryProductOptionDTO>> GetProductOptionsByProductIdAsync(PaginationParams pagination, Guid productId)
        {
            var product = await _productQueryHandler.GetProductByIdAsync(productId);
            if (product == null) return null;

            var results = await _connectionProxy.QueryAsync<QueryProductOptionDTO>(SQLStatements.GetProductOptionByProductId,
                          new { ProductId = productId, Offset = pagination.Offset, Limit = pagination.Limit });

            return results;
        }
    }
}
