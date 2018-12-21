using System;
using System.Data;

namespace Kts.RefactorThis.Application.Abstractions
{
    /// <summary>
    /// Represents a unit of work that manages a single database transaction
    /// Instances of classes that implement this interface should not be registered in DI service.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Creates a new transaction
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Commit changes
        /// </summary>
        void Commit();

        /// <summary>
        /// Rolls back changes
        /// </summary>
        void Rollback();

        /// <summary>
        /// Returns a transaction object
        /// </summary>
        /// <returns></returns>
        IDbTransaction GetCurrentTransaction(bool throwIfNull = false);
    }
}
