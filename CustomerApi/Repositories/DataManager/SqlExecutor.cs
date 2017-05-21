using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace CustomerApi.Repositories.DataManager
{
    public class SqlExecutor : IDbExecutor
    {
        readonly IDbConnection _dbConnection;

        public SqlExecutor(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public int Execute(
            string sql,
            object param = null,
            IDbTransaction transaction = null,
            int? commandTimeout = default(int?),
            CommandType? commandType = default(CommandType?))
        {
            try
            {
                var a = _dbConnection.Execute(
                              sql,
                              param,
                              transaction,
                              commandTimeout,
                              commandType);
                return a;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public IEnumerable<dynamic> Query(
            string sql,
            object param = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            int? commandTimeout = default(int?),
            CommandType? commandType = default(CommandType?))
        {
            return _dbConnection.Query(
                sql,
                param,
                transaction,
                buffered,
                commandTimeout,
                commandType);
        }

        public IEnumerable<T> Query<T>(
            string sql,
            object param = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            int? commandTimeout = default(int?),
            CommandType? commandType = default(CommandType?))
        {
            try
            {
                var a = _dbConnection.Query<T>(
                sql,
                param,
                transaction,
                buffered,
                commandTimeout,
                commandType);
                return a;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<Dapper.SqlMapper.GridReader> QueryMultipleAsync(
            string sql,
            object param = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            int? commandTimeout = default(int?),
            CommandType? commandType = default(CommandType?))
        {
            try
            {
                var a = await _dbConnection.QueryMultipleAsync(
                sql,
                param,
                transaction,
                commandTimeout,
                commandType);
                return a;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            try
            {
                var a = await _dbConnection.ExecuteAsync(
                              sql,
                              param,
                              transaction,
                              commandTimeout,
                              commandType);
                return a;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            try
            {
                var a = await _dbConnection.QueryAsync<T>(
                sql,
                param,
                transaction,
                commandTimeout,
                commandType);
                return a;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void Dispose()
        {
            _dbConnection.Dispose();
        }
        public void Open()
        {
            _dbConnection.Open();
        }
    }
}