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

        public async Task<int> AddEdit(CompoundUnit entity,CancellationToken cancellationToken, int userId)
        {
            int res = 0;

            using SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();
            cmd.CommandText = "[SP_COMPOUND_UNIT_ADD_EDIT]";
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter result = new SqlParameter("@return", dbType: SqlDbType.VarChar, 200);
            result.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(result);

            cmd.Parameters.AddWithValue("@id", entity.ID);
            cmd.Parameters.AddWithValue("@UnitID", entity.UnitID);
            cmd.Parameters.AddWithValue("@ParentUnitID", entity.ParentUnitID);
            cmd.Parameters.AddWithValue("@RelationValue", entity.RelationValue);
            cmd.Parameters.AddWithValue("@Remarks", entity.Remarks);
           
            cmd.Parameters.AddWithValue("@UserID", userId);

            _dbConnection.Open();
            res = await cmd.ExecuteNonQueryAsync();
            await cmd.ExecuteNonQueryAsync(cancellationToken);
            res = Convert.ToInt32(result.Value);

            return res;

        }

        public async Task<decimal?> ConvertUnit(int defaultUnitID, int currentUnitID, decimal valueToConvert, CancellationToken cancellationToken)
        {
            using (_dbConnection as SqlConnection)
            {
                using SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();

                cmd.CommandText = "SP_CONVERT_COMPOUND_UNIT";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@defUnitID", defaultUnitID);
                cmd.Parameters.AddWithValue("@currUnitID", currentUnitID);
                cmd.Parameters.AddWithValue("@actualValue", valueToConvert);

                _dbConnection.Open();
                var value = await cmd.ExecuteScalarAsync(cancellationToken);
            return value == DBNull.Value ? null : Convert.ToDecimal(value);
            }
        }

        public async Task<int> Delete(int id, CancellationToken cancellationToken, int userId)
        {
            using (_dbConnection as SqlConnection)
            {
                using SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();

                cmd.CommandText = "[SP_COMPOUNT_UNIT_DELETE]";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);

                _dbConnection.Open();
                return await cmd.ExecuteNonQueryAsync(cancellationToken);
            }
        }

        public async Task<List<CompoundUnit>> Get(CancellationToken cancellationToken)
        {
            List<CompoundUnit> listEntity = new List<CompoundUnit>();

            using (_dbConnection as SqlConnection)
            {
                using SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();

                cmd.CommandText = $"SP_GET_COMPOUND_UNIT";
                cmd.CommandType = CommandType.StoredProcedure;

                _dbConnection.Open();

                using IDataReader rdr = await cmd.ExecuteReaderAsync(cancellationToken);
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

        public async Task<CompoundUnit> Get(int id, CancellationToken cancellationToken)
        {
            CompoundUnit entity = new();

            using (_dbConnection as SqlConnection)
            {
                using SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();

                cmd.CommandText = $"SP_GET_COMPOUND_UNIT";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID", id);

                _dbConnection.Open();

                using IDataReader rdr = await cmd.ExecuteReaderAsync(cancellationToken);
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

        public async Task<List<UnitDetails>> GetRelatedUnit(int BaseUnitID, CancellationToken cancellationToken)
        {
            List<UnitDetails> entity = new List<UnitDetails>();

            using (_dbConnection as SqlConnection)
            {
                using SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();

                cmd.CommandText = "SP_GET_UNIT_CONVERSION_RATES";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BaseUnitID", BaseUnitID);

                _dbConnection.Open();

                using IDataReader rdr = await cmd.ExecuteReaderAsync(cancellationToken);
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

        public async Task<List<UnitDetails>> GetMultipleRelatedUnit(string baseUnits, CancellationToken cancellationToken)
        {
            List<UnitDetails> entity = new List<UnitDetails>();

            using (_dbConnection as SqlConnection)
            {
                using SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();

                cmd.CommandText = "SP_GET_MULTIPLE_UNIT_CONVERSION_RATES";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@unitsCSV", baseUnits);

                _dbConnection.Open();

                using IDataReader rdr = await cmd.ExecuteReaderAsync(cancellationToken);
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
