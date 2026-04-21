using inventory_system_api.Application.IRepository;
using inventory_system_api.Application.Models.Inventory;
using inventory_system_api.Application.Models.System;
using Microsoft.Data.SqlClient;
using System.Data;

namespace inventory_system_api.Infrastructure.Repository
{
    public class CompoundUnitRepository : ICompoundUnitRepository
    {
        private readonly IDbConnection _dbConnection;

        public CompoundUnitRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<int> AddEdit(CompoundUnit entity)
        {
            int res = 0;

            using SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();
            cmd.CommandText = "[SP_COMPOUND_UNIT_ADD_EDIT]";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id", entity.ID);
            cmd.Parameters.AddWithValue("@UnitID", entity.UnitID);
            cmd.Parameters.AddWithValue("@ParentUnitID", entity.ParentUnitID);
            cmd.Parameters.AddWithValue("@RelationValue", entity.RelationValue);
            cmd.Parameters.AddWithValue("@Remarks", entity.Remarks);
           
            cmd.Parameters.AddWithValue("@User", "root");

            _dbConnection.Open();
            res = await cmd.ExecuteNonQueryAsync();

            return res;

        }

        public async Task<decimal?> ConvertUnit(int defaultUnitID, int currentUnitID, decimal valueToConvert)
        {
            using (_dbConnection as SqlConnection)
            {
                using SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();

                cmd.CommandText = "SELECT [fnConvertCompoundUnit](@defUnitID, @currUnitID, @actualValue, 1)";
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

                cmd.CommandText = "[Inv].[spUnitDelete]";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);

                _dbConnection.Open();
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<List<CompoundUnit>> Get()
        {
            List<CompoundUnit> listEntity = new List<CompoundUnit>();

            using (_dbConnection as SqlConnection)
            {
                using SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();

                cmd.CommandText = $"select CompoundUnitID, UnitID, cu.ParentUnitID, u.UnitName, pu.UnitName ParentUnitName, RelationValue, cu.Remarks from tblCompoundUnit cu " +
                    $"inner join tblUnitMaintenance u on cu.UnitID = u.UnitMaintenanceID inner join tblUnitMaintenance pu on cu.ParentUnitID = pu.UnitMaintenanceID";
                cmd.CommandType = CommandType.Text;

                _dbConnection.Open();

                using IDataReader rdr = await cmd.ExecuteReaderAsync();
                while (rdr.Read())
                {
                    listEntity.Add(new CompoundUnit
                    {
                        ID = Convert.ToInt32(rdr["CompoundUnitID"]),
                        UnitID = Convert.ToInt32(rdr["UnitID"]),
                        UnitName = rdr["UnitName"].ToString()?? "",
                        ParentUnitID = Convert.ToInt32(rdr["ParentUnitID"]),
                        RelationValue = Convert.ToDecimal(rdr["RelationValue"]),
                        ParentUnitName = rdr["ParentUnitName"].ToString() ?? "",
                        Remarks = rdr["Remarks"].ToString() ?? "",
                    });
                }
            }
            return listEntity;
        }

        public async Task<CompoundUnit> Get(int id)
        {
            CompoundUnit entity = new();

            using (_dbConnection as SqlConnection)
            {
                using SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();

                cmd.CommandText = $"select CompoundUnitID, UnitID, u.UnitName, cu.ParentUnitID, pu.UnitName ParentUnitName, RelationValue, cu.Remarks from tblCompoundUnit cu " +
                    $"inner join tblUnitMaintenance u on cu.UnitID = u.UnitMaintenanceID inner join tblUnitMaintenance pu on cu.ParentUnitID = pu.UnitMaintenanceID where CompoundUnitID = @id";
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@id", id);

                _dbConnection.Open();

                using IDataReader rdr = await cmd.ExecuteReaderAsync();
                while (rdr.Read())
                {
                    entity = new CompoundUnit
                    {
                        ID = Convert.ToInt32(rdr["CompoundUnitID"]),
                        UnitID = Convert.ToInt32(rdr["UnitID"]),
                        UnitName = rdr["UnitName"].ToString()??"",
                        ParentUnitID = Convert.ToInt32(rdr["ParentUnitID"]),
                        RelationValue = Convert.ToDecimal(rdr["RelationValue"]),
                        ParentUnitName = rdr["ParentUnitName"].ToString()?? "",
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

                cmd.CommandText = "spGetMultipleUnitConversionRates";
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
