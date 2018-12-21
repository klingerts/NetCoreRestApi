using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Kts.RefactorThis.Common;
using Kts.RefactorThis.Common.DependencyMarkers;

namespace Kts.RefactorThis.DataAccess
{
    /// <summary>
    /// Encapsulates the creation of a database connection.
    /// </summary>
    public class ConnectionProxy : IConnectionProxy, IPerRequestDependency
    {
        private IDbConnection _connection;

        public ConnectionProxy(ConnectionStrings connectionStrings)
        {
            _connection = new SqlConnection(connectionStrings.Default);
        }

        // Returns an open database connection
        public virtual IDbConnection GetCurrentConnection(bool returnOpenConnection = true)
        {
            if (_connection.State == ConnectionState.Closed && returnOpenConnection)
            {                
                _connection.Open();
            }
            return _connection;
        }

        public virtual async Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null)
        {
            return await _connection.ExecuteAsync(sql, param, transaction);
        }

        public virtual async Task<IEnumerable<Guid>> CreateAsync<T>(string sql, object param = null, IDbTransaction transaction = null)
        {
            return await _connection.QueryAsync<Guid>(sql, param, transaction);
        }

        public virtual async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null)
        {
            return await _connection.QueryAsync<T>(sql, param);
        }

        public virtual async Task<T> QueryFirstAsync<T>(string sql, object param = null)
        {
            return await _connection.QueryFirstAsync<T>(sql, param);
        }

        public virtual async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null)
        {
            return await _connection.QueryFirstOrDefaultAsync<T>(sql, param);
        }

        #region IDisposable Support
        private bool _isDisposed = false; 
        
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _connection?.Dispose();
                }

                _connection = null;
                _isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public Task<IEnumerable<Guid>> CreateAsync(string sql, object parm = null, IDbTransaction transaction = null)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
