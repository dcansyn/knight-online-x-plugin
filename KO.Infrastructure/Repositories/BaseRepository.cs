using Dapper;
using KO.Core.Constants.Query;
using KO.Core.Helpers.Storage;
using KO.Core.Models.Query;
using KO.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace KO.Infrastructure.Repositories
{
    public abstract class BaseRepository<TEntity> : IDisposable
    {
        public SQLiteConnection Connection { get; set; }
        public string Table => typeof(TEntity).GetTableName();
        public string Columns => typeof(TEntity).GetColumnNames();
        public string Parameters => Columns.GetColumnParameters();

        public BaseRepository()
        {
            Connection = new SQLiteConnection(StorageHelper.GetDataConnectionString());
        }

        public async Task<bool> Create(TEntity entity)
        {
            return await Connection.ExecuteAsync(string.Format(Commands.Insert, Table, Columns, Parameters), entity) > 0;
        }

        public async Task<bool> Update(TEntity entity, string columns = null)
        {
            return await Connection.ExecuteAsync(string.Format(Commands.Update, Table, (columns ?? Columns).GetUpdateColumns()), entity) > 0;
        }

        public async Task<bool> Delete(TEntity entity)
        {
            return await Connection.ExecuteAsync(string.Format(Commands.Update, Table), entity) > 0;
        }

        public async Task<int> Count(params Parameter[] parameters)
        {
            var list = new Dictionary<string, object>();
            var query = list.GetQuery(parameters);

            return await Connection.ExecuteScalarAsync<int>(string.Format(Commands.Count, Table, query), new DynamicParameters(list));
        }

        public async Task<TEntity> Get(params Parameter[] parameters)
        {
            var list = new Dictionary<string, object>();
            var query = list.GetQuery(parameters);

            return await Connection.QuerySingleOrDefaultAsync<TEntity>(string.Format(Commands.Where, Table, query), new DynamicParameters(list));
        }

        public async Task<TEntity[]> All(params Parameter[] parameters)
        {
            var list = new Dictionary<string, object>();
            var query = list.GetQuery(parameters);

            return (await Connection.QueryAsync<TEntity>(string.Format(Commands.Where, Table, query), new DynamicParameters(list))).ToArray();
        }

        public void Dispose()
        {
            Connection.Close();
            Connection.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
