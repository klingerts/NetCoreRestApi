using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Kts.RefactorThis.Application.Abstractions;
using Kts.RefactorThis.Common.DependencyMarkers;
using Kts.RefactorThis.DTO;

namespace Kts.RefactorThis.DataAccess.Repositories
{
    /// <summary>
    /// Provides functionality to read (by id)/create/update/delete records in ProductOption table
    /// </summary>
    public class ProductOptionRepository : IProductOptionRepository, IPerRequestDependency
    {
        private readonly IConnectionProxy _connectionProxy;
        public ProductOptionRepository(IConnectionProxy connectionProxy)
        {
            _connectionProxy = connectionProxy;
        }

        // Get optionProduct by productid and optionid
        public virtual async Task<QueryProductOptionDTO> GetProductOptionByIdAsync(Guid productId, Guid optionId)
        {
            var option = await _connectionProxy.QueryFirstOrDefaultAsync<QueryProductOptionDTO>(SQLStatements.GetProductOptionByIdAndProductId,
                         new { ProductId = productId, OptionId = optionId });

            return option;
        }

        // Returns id for new product
        public virtual async Task<Guid> CreateAsync(CreateProductOptionDTO productOptionToCreate, IUnitOfWork unitOfWork)
        {
            var result = await _connectionProxy.CreateAsync(SQLStatements.CreateProductOption, 
                                                     productOptionToCreate, 
                                                     unitOfWork?.GetCurrentTransaction());

            var id = result.First();
            
            return id;
        }

        // Returns true when row updated, false if no row uptated
        public virtual async Task<bool> UpdateAsync(UpdateProductOptionDTO productOption, IUnitOfWork unitOfWork)
        {
            var updateCount = await _connectionProxy.ExecuteAsync(SQLStatements.UpdateProductOptionById, 
                                                      productOption, 
                                                      unitOfWork?.GetCurrentTransaction());

            return updateCount > 0;
        }

        // Returns true when row deleted, false if no row deleted
        public virtual async Task<bool> DeleteAsync(Guid productId, Guid optionId, IUnitOfWork unitOfWork)
        {
            var deleteCount = await _connectionProxy.ExecuteAsync(SQLStatements.DeleteProductOptionByIdAndProductId, 
                                                      new { ProductId = productId, OptionId = optionId },
                                                      unitOfWork?.GetCurrentTransaction());

            return deleteCount > 0;
        }
    }
}
