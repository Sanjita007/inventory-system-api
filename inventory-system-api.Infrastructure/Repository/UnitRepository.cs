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
            //int res = 0;

            //using SqlConnection conn = new SqlConnection(_connectionString);
            //using SqlCommand cmd = conn.CreateCommand();
            //cmd.CommandText = "[Inv].[spProductAddEdit]";
            //cmd.CommandType = CommandType.StoredProcedure;

            //cmd.Parameters.AddWithValue("@id", entity.ID);
            //cmd.Parameters.AddWithValue("@EngName", entity.EngName);
            //cmd.Parameters.AddWithValue("@NepName", entity.NepName);
            //cmd.Parameters.AddWithValue("@GroupID", entity.GroupID);
            //cmd.Parameters.AddWithValue("@Code", entity.Code);
            //cmd.Parameters.AddWithValue("@Color", entity.BackColor);
            //cmd.Parameters.AddWithValue("@DepotID", entity.DepotID);
            //cmd.Parameters.AddWithValue("@UnitID", entity.UnitID);
            //cmd.Parameters.AddWithValue("@IsVatApplicable", entity.IsVatApplicable);
            //cmd.Parameters.AddWithValue("@IsActive", entity.IsActive);
            //cmd.Parameters.AddWithValue("@CompanyID", entity.CompanyID);
            //cmd.Parameters.AddWithValue("@Size", entity.Size);
            //cmd.Parameters.AddWithValue("@OpenPurchaseQty", entity.PurchaseQuantity);
            //cmd.Parameters.AddWithValue("@PurchaseRate", entity.PurchaseRate);
            //cmd.Parameters.AddWithValue("@TaxID", entity.TaxID);
            //cmd.Parameters.AddWithValue("@UserID", "root");

            //await conn.OpenAsync();
            //res = await cmd.ExecuteNonQueryAsync();

            //return res;

            return -1;
        }

        public async Task<decimal?> ConvertUnit(int defaultUnitID, int currentUnitID, decimal valueToConvert)
        {
            using (_dbConnection as SqlConnection)
            {
                using SqlCommand cmd = _dbConnection.CreateCommand() as SqlCommand;

                cmd.CommandText = "SELECT [System].[fnConvertCompoundUnit](@defUnitID, @currUnitID, @actualValue, 1)";
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
                using SqlCommand cmd = _dbConnection.CreateCommand() as SqlCommand;

                cmd.CommandText = "[Inv].[spUnitDelete]";
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
                using SqlCommand cmd = _dbConnection.CreateCommand() as SqlCommand;

                cmd.CommandText = "SELECT * FROM System.tblUnitMaintenance WHERE CompanyID = 1";
                cmd.CommandType = CommandType.Text;

                _dbConnection.Open();

                using IDataReader rdr = await cmd.ExecuteReaderAsync();
                while (rdr.Read())
                {
                    listEntity.Add(new Unit
                    {
                        ID = Convert.ToInt32(rdr["UnitMaintenanceID"]),
                        Name = rdr["UnitName"].ToString(),
                        Symbol = rdr["Symbol"].ToString(),
                        Remarks = rdr["Remarks"].ToString(),
                    });
                }
            }
            return listEntity;
        }

        public async Task<Unit> Get(int id)
        {
            Unit entity= null;

            using (_dbConnection as SqlConnection)
            {
                using SqlCommand cmd = _dbConnection.CreateCommand() as SqlCommand;

                cmd.CommandText = "SELECT * FROM System.tblUnitMaintenance WHERE UnitMaintenanceID = @id and CompanyID = 1";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@id", id);

                _dbConnection.Open();

                using IDataReader rdr = await cmd.ExecuteReaderAsync();
                while (rdr.Read())
                {
                    entity = new Unit
                    {
                        ID = Convert.ToInt32(rdr["UnitMaintenanceID"]),
                        Name = rdr["UnitName"].ToString(),
                        Symbol = rdr["Symbol"].ToString(),
                        Remarks = rdr["Remarks"].ToString(),
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
                using SqlCommand cmd = _dbConnection.CreateCommand() as SqlCommand;

                cmd.CommandText = "System.spGetUnitConversionRates";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BaseUnitID", BaseUnitID);

                _dbConnection.Open();

                using IDataReader rdr = await cmd.ExecuteReaderAsync();
                while (rdr.Read())
                {
                    entity.Add(new UnitDetails
                    {
                        ID = Convert.ToInt32(rdr["UnitID"]),
                        Name = rdr["UnitName"].ToString(),
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
                using SqlCommand cmd = _dbConnection.CreateCommand() as SqlCommand;

                cmd.CommandText = "System.spGetMultipleUnitConversionRates";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@unitsCSV", baseUnits);

                _dbConnection.Open();

                using IDataReader rdr = await cmd.ExecuteReaderAsync();
                while (rdr.Read())
                {
                    entity.Add(new UnitDetails
                    {
                        ID = Convert.ToInt32(rdr["UnitID"]),
                        DefaultUnitID = Convert.ToInt32(rdr["DefaultUnitID"]),
                        Name = rdr["UnitName"].ToString(),
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
