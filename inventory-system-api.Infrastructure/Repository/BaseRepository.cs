using Microsoft.Data.SqlClient;
using System.Data;

namespace inventory_system_api.Infrastructure.Repository
{
    public abstract class BaseRepository<T>
    {
        IDbConnection _dbConnection;
        protected BaseRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        protected async Task<List<T>> ExecuteQueryAsync(string query, Func<IDataReader, T> mapFunction, SqlParameter[] parameters, CommandType commandType = CommandType.Text)
        {
            var entities = new List<T>();
            using (var conn = _dbConnection as SqlConnection)
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = commandType;
                    cmd.CommandText = query;
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            entities.Add(mapFunction(reader));
                        }
                    }

                    conn.Close();
                }
            }

            return entities;
        }


        protected async Task<int> ExecuteNonQueryAsync(string query, SqlParameter[] parameters, CommandType commandType = CommandType.Text)
        {
            int res = -1;
            using (var conn = _dbConnection as SqlConnection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = commandType;
                    cmd.CommandText = query;
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    res = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

            return res;
        }

        public abstract T MapEntity(IDataReader reader);
    }
}