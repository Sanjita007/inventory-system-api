using inventory_system_api.Application.IRepository;
using inventory_system_api.Application.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace inventory_system_api.Infrastructure.Repository.Inventory
{
    public class ErrorLogRepositoy : IErrorLogRepository
    {
        IDbConnection _dbConnection;
        public ErrorLogRepositoy(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<int> AddErrorLog(ErrorLog entity)
        {
            int res = 0;
            using (_dbConnection as SqlConnection)
            {
                SqlParameter result = new("@return", dbType: SqlDbType.VarChar, 200);
                result.Direction = ParameterDirection.Output;

                SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();
                cmd.CommandText = "[SYSTEM].[SP_API_ERROR_LOG_ADD_EDIT]";
                cmd.Parameters.AddWithValue("@id", entity.ID);
                cmd.Parameters.AddWithValue("@REQUESTMETHOD", entity.RequestMethod);
                cmd.Parameters.AddWithValue("@REQUESTURI", entity.RequestPath);
                cmd.Parameters.AddWithValue("@REQUESTHEADER", entity.RequestHeader);
                cmd.Parameters.AddWithValue("@REQUESTBODY", entity.RequestBody);
                cmd.Parameters.AddWithValue("@TIMEUTC", entity.DateTimeUtc);
                cmd.Parameters.AddWithValue("@MESSAGE", entity.ErrorMessage);
                
                cmd.Parameters.AddWithValue("@UserID", "root");
                cmd.Parameters.Add(result);

                cmd.CommandType = CommandType.StoredProcedure;
                _dbConnection.Open();
                res = await cmd.ExecuteNonQueryAsync();

            }

            return res;
        }


        public async Task<List<ErrorLog>> Get()
        {

            List<ErrorLog> listEntity = [];
            using (_dbConnection as SqlConnection)
            {
                SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();
                cmd.CommandText = "select * from System.TBLAPIERRORLOG";
                cmd.CommandType = CommandType.Text;
                _dbConnection.Open();
                IDataReader rdr = await cmd.ExecuteReaderAsync();

                while (rdr.Read())
                {
                    ErrorLog entity = new()
                    {
                        RequestMethod = rdr["REQUESTMETHOD"].ToString() ?? "",
                        RequestPath = rdr["REQUESTURI"].ToString() ?? "",
                        RequestHeader = rdr["REQUESTHEADER"].ToString() ?? "",
                        ErrorMessage = rdr["MESSAGE"].ToString() ?? "",
                        DateTimeUtc = rdr["TIMEUTC"] != DBNull.Value ? null : Convert.ToDateTime(rdr["TIMEUTC"])
                    };

                    listEntity.Add(entity);
                }
                _dbConnection.Close();
            }
            return listEntity;
        }



    }
}

