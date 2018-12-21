using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Kts.RefactorThis.DataAccess
{
    /// <summary>
    /// Encapsulates the creation of a database connection
    /// Supports testability
    /// </summary>
    public interface IConnectionProxy : IDisposable
    {
        // Returns an open database connection
        IDbConnection GetCurrentConnection(bool returnOpenConnection = true);

        // Done this way so that we can get the Guid back
        Task<IEnumerable<Guid>> CreateAsync(string sql, object parm = null, IDbTransaction transaction = null);

        // Use this for updates and deletes
        Task<int> ExecuteAsync(string sql, object parm = null, IDbTransaction transaction = null);

        // Regular queries (no transaction)
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object parm = null);
        Task<T> QueryFirstAsync<T>(string sql, object parm = null);
        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object parm = null);
    }
}