using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kts.RefactorThis.Application.Queries;
using Kts.RefactorThis.Common;
using Kts.RefactorThis.Common.DependencyMarkers;
using Kts.RefactorThis.DTO;

namespace Kts.RefactorThis.DataAccess.QueryHandlers
{
    /// <summary>
    /// Provides functionality to query products
    /// </summary>
    public class ProductQueryHandler : IProductQueryHandler, IPerRequestDependency
    {
        private readonly IConnectionProxy _connectionProxy;
        public ProductQueryHandler(IConnectionProxy connectionProxy)
        {
            _connectionProxy = connectionProxy;
        }

        // Get product by id
        public virtual async Task<QueryProductDTO> GetProductByIdAsync(Guid id)
        {
            var product = await _connectionProxy.QueryFirstOrDefaultAsync<QueryProductDTO>(SQLStatements.GetProductById,
                                                                                           new { Id = id });

            return product;
        }

        // Get products with options filter by name (if provided).
        public virtual async Task<IEnumerable<QueryProductDTO>> GetProductsAsync(PaginationParams pagination, string name = null)
        {
            string sql = name == null
                          ? SQLStatements.GetAllProducts
                          : SQLStatements.GetProductsByName;

            var results = await _connectionProxy.QueryAsync<QueryProductDTO>(sql,
                           new { Name = name, Offset = pagination.Offset, Limit = pagination.Limit });

            return results;
        }
    }
}
