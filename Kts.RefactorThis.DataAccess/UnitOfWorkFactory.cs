using Kts.RefactorThis.Application.Abstractions;
using Kts.RefactorThis.Common.DependencyMarkers;

namespace Kts.RefactorThis.DataAccess
{
    /// <summary>
    /// Enables the creation of multi-level unit of work (sub transaction) that share the same database connection.
    /// </summary>
    public class UnitOfWorkFactory : IUnitOfWorkFactory, IPerRequestDependency
    {
        private readonly IConnectionProxy _connectionProxy;

        public UnitOfWorkFactory(IConnectionProxy connectionProxy)
        {
            _connectionProxy = connectionProxy;
        }

        /// <summary>
        /// Returns a new unit of work instance
        /// </summary>
        /// <returns></returns>
        public virtual IUnitOfWork GetNewUnitOfWork()
        {
            return new UnitOfWork(_connectionProxy);
        }
    }
}
