using System;
using System.Data;
using Kts.RefactorThis.Application.Abstractions;

namespace Kts.RefactorThis.DataAccess
{
    /// <summary>
    /// Represents a unit of work that manages a single database transaction
    /// Class marked as internal so that it can only by UnitOfWorkFactory
    /// </summary>
    internal class UnitOfWork : IUnitOfWork
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction = null;

        public UnitOfWork(IConnectionProxy connectionProxy)
        {
            _connection = connectionProxy.GetCurrentConnection(returnOpenConnection: false);
        }

        /// <exception cref="InvalidOperationException">When called more than one time</exception>
        public virtual void BeginTransaction()
        {
            if (_transaction != null) throw new InvalidOperationException("Transaction already started for this unit of work");

            if (_connection.State == ConnectionState.Closed) _connection.Open();

            _transaction = _connection.BeginTransaction();
        }

        /// <exception cref="InvalidOperationException">When called before transaction is started</exception>
        public virtual void Commit()
        {
            if (_transaction == null) throw new InvalidOperationException("Cannot commit before beginning transaction.");

            _transaction.Commit();
        }

        /// <exception cref="InvalidOperationException">When called before transaction is started</exception>
        public virtual void Rollback()
        {
            if (_transaction == null) throw new InvalidOperationException("Cannot rollback before beginning transaction.");

            _transaction.Rollback();
        }
        
        // Returns current transaction object, or null if transaction has not started
        public virtual IDbTransaction GetCurrentTransaction(bool throwIfNull = false)
        {
            if (throwIfNull && _transaction == null) throw new InvalidOperationException("Transaction not started.");

            return _transaction;
        }

        #region IDisposable Support
        private bool _isDisposed = false; 

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing)
            {
                // Do not dispose connection as it will be disposed by the proxy
                _transaction?.Dispose();
            }

            _transaction = null;
            _connection = null;
            _isDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }

}

