using System;
using System.Linq;
using System.Threading.Tasks;
using Kts.RefactorThis.Application.Abstractions;
using Kts.RefactorThis.Common.DependencyMarkers;
using Kts.RefactorThis.DTO;

namespace Kts.RefactorThis.DataAccess.Repositories
{
    /// <summary>
    /// Provides functionality to create/update/delete records in Product table
    /// </summary>
    public class ProductRepository : IProductRepository, IPerRequestDependency
    {
        private readonly IConnectionProxy _connectionProxy;
        public ProductRepository(IConnectionProxy connectionProxy)
        {
            _connectionProxy = connectionProxy;
        }

        // Returns id for new row
        public virtual async Task<Guid> CreateAsync(CreateProductDTO productToCreate, 
                                                    IUnitOfWork unitOfWork)
        {
            var result = await _connectionProxy.CreateAsync(SQLStatements.CreateProduct, 
                                                     productToCreate, 
                                                     unitOfWork?.GetCurrentTransaction());
            return result.First();
        }

        // Returns true when row updated, false if no row uptated
        public virtual async Task<bool> UpdateAsync(UpdateProductDTO productToUpdate, 
                                                    IUnitOfWork unitOfWork)
        {
            var updateCount = await _connectionProxy.ExecuteAsync(SQLStatements.UpdateProductById, 
                                                      productToUpdate, 
                                                      unitOfWork?.GetCurrentTransaction());
            return updateCount > 0;
        }

        // Returns true when row deleted, false if no row deleted
        public virtual async Task<bool> DeleteAsync(Guid idToDelete, 
                                                    IUnitOfWork unitOfWork)
        {
            var deleteCount = await _connectionProxy.ExecuteAsync(
                                  SQLStatements.DeleteProductById,
                                  new { Id = idToDelete },
                                  unitOfWork.GetCurrentTransaction(true));

            return deleteCount > 0;
        }
    }
}
