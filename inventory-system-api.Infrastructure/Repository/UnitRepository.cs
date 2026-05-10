using inventory_system_api.Application.IRepository;
using inventory_system_api.Application.Models.Inventory;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace inventory_system_api.Infrastructure.Repository
{
    public class UnitRepository : IUnitRepository
    {
        private readonly IDbConnection _dbConnection;

        public UnitRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<int> AddEdit(Unit entity)
        {
            int res = 0;

            using (_dbConnection as SqlConnection)
            {
                using SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();
                cmd.CommandText = "[SP_UNIT_ADD_EDIT]";
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter result = new SqlParameter("@return", dbType: SqlDbType.VarChar, 200);
                result.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(result);

                cmd.Parameters.AddWithValue("@id", entity.ID);
                cmd.Parameters.AddWithValue("@Name", entity.Name);
                cmd.Parameters.AddWithValue("@Symbol", entity.Symbol);
                cmd.Parameters.AddWithValue("@Remarks", entity.Remarks);
                
                cmd.Parameters.AddWithValue("@UserID", "root");

                _dbConnection.Open();
                await cmd.ExecuteNonQueryAsync();
                res = Convert.ToInt32(result.Value);

            }
            return res;

        }

        public async Task<decimal?> ConvertUnit(int defaultUnitID, int currentUnitID, decimal valueToConvert)
        {
            using (_dbConnection as SqlConnection)
            {
                using SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();

                cmd.CommandText = "SELECT dbo.[FN_CONVERT_COMPOUND_UNIT](@defUnitID, @currUnitID, @actualValue, 1)";
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@defUnitID", defaultUnitID);
                cmd.Parameters.AddWithValue("@currUnitID", currentUnitID);
                cmd.Parameters.AddWithValue("@actualValue", valueToConvert);

                _dbConnection.Open();
                var value = await cmd.ExecuteScalarAsync();
            return value == DBNull.Value ? null : Convert.ToDecimal(value);
            }
        }

        public async Task<int> Delete(int id)
        {
            using (_dbConnection as SqlConnection)
            {
                using SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();

                cmd.CommandText = "[SP_UNIT_DELETE]";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);

                _dbConnection.Open();
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<List<Unit>> Get()
        {
            List<Unit> listEntity = new List<Unit>();

            using (_dbConnection as SqlConnection)
            {
                using SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();

                cmd.CommandText = "SELECT * FROM UNIT WHERE CompanyID = 1";
                cmd.CommandType = CommandType.Text;

                _dbConnection.Open();

                using IDataReader rdr = await cmd.ExecuteReaderAsync();
                while (rdr.Read())
                {
                    listEntity.Add(new Unit
                    {
                        ID = Convert.ToInt32(rdr["UnitMaintenanceID"]),
                        Name = rdr["UnitName"].ToString() ?? "",
                        Symbol = rdr["Symbol"].ToString() ?? "",
                        Remarks = rdr["Remarks"].ToString() ?? "",
                    });
                }
            }
            return listEntity;
        }

        public async Task<Unit> Get(int id)
        {
            Unit entity= new();

            using (_dbConnection as SqlConnection)
            {
                using SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();

                cmd.CommandText = "SELECT * FROM UNIT WHERE UnitMaintenanceID = @id and CompanyID = 1";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@id", id);

                _dbConnection.Open();

                using IDataReader rdr = await cmd.ExecuteReaderAsync();
                while (rdr.Read())
                {
                    entity = new Unit
                    {
                        ID = Convert.ToInt32(rdr["UnitMaintenanceID"]),
                        Name = rdr["UnitName"].ToString()??"",
                        Symbol = rdr["Symbol"].ToString() ?? "",
                        Remarks = rdr["Remarks"].ToString() ?? "",
                    };
                }
            }
            return entity;
        }

        public async Task<List<UnitDetails>> GetRelatedUnit(int BaseUnitID)
        {
            List<UnitDetails> entity = new List<UnitDetails>();

            using (_dbConnection as SqlConnection)
            {
                using SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();

                cmd.CommandText = "spGetUnitConversionRates";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BaseUnitID", BaseUnitID);

                _dbConnection.Open();

                using IDataReader rdr = await cmd.ExecuteReaderAsync();
                while (rdr.Read())
                {
                    entity.Add(new UnitDetails
                    {
                        ID = Convert.ToInt32(rdr["UnitID"]),
                        Name = rdr["UnitName"].ToString()??"",
                        DefaultUnitID =  rdr["DefaultUnitID"] == DBNull.Value ? 0 : Convert.ToInt32(rdr["DefaultUnitID"]),
                        ConversionRate = rdr["ConversionRate"] == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(rdr["ConversionRate"])
                    });
                }
            }
            return entity;
        }

        public async Task<List<UnitDetails>> GetMultipleRelatedUnit(string baseUnits)
        {
            List<UnitDetails> entity = new List<UnitDetails>();

            using (_dbConnection as SqlConnection)
            {
                using SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();

                cmd.CommandText = "SP_GET_MULTIPLE_UNIT_CONVERSION_RATES";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UNITSCSV", baseUnits);

                _dbConnection.Open();

                using IDataReader rdr = await cmd.ExecuteReaderAsync();
                while (rdr.Read())
                {
                    entity.Add(new UnitDetails
                    {
                        ID = Convert.ToInt32(rdr["UnitID"]),
                        DefaultUnitID = Convert.ToInt32(rdr["DefaultUnitID"]),
                        Name = rdr["UnitName"].ToString() ?? "",
                        ConversionRate = rdr["ConversionRate"] == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(rdr["ConversionRate"])
                    });
                }
            }
            return entity;
        }
    }

}
