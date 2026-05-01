using inventory_system_api.Application.IRepository;
using inventory_system_api.Application.Models.System;
using Microsoft.Data.SqlClient;
using System.Data;

namespace inventory_system_api.Infrastructure.Repository
{
    public class TaxRepository : ITaxRepository
    {
        IDbConnection _dbConnection;
        public TaxRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<int> AddEdit(Tax entity)
        {
            int res = 0;
            using (_dbConnection as SqlConnection)
            {
                SqlParameter result = new SqlParameter("@return", dbType: SqlDbType.VarChar, 200);
                result.Direction = ParameterDirection.Output;

                SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();
                cmd.CommandText = "[SP_TAX_ADD_EDIT]";
                cmd.Parameters.AddWithValue("@id", entity.ID);
                cmd.Parameters.AddWithValue("@Name", entity.Name);
                cmd.Parameters.AddWithValue("@Code", entity.Code);
                cmd.Parameters.AddWithValue("@Rate1", entity.Rate);
                cmd.Parameters.AddWithValue("@User", "root");
                cmd.Parameters.Add(result);

                cmd.CommandType = CommandType.StoredProcedure;
                _dbConnection.Open();
                res = await cmd.ExecuteNonQueryAsync();

            }

            return res;
        }

        public async Task<int> Delete(int id)
        {

            using (_dbConnection as SqlConnection)
            {
                SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();
                cmd.CommandText = "[SP_TAX_DELETE]";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.CommandType = CommandType.StoredProcedure;
                _dbConnection.Open();
                return await cmd.ExecuteNonQueryAsync();

            }
        }

        public async Task<List<Tax>> Get()
        {

            List<Tax> listEntity = [];
            using (_dbConnection as SqlConnection)
            {
                SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();
                cmd.CommandText = "select * from TAX where CompanyID = 1";
                cmd.CommandType = CommandType.Text;
                _dbConnection.Open();
                IDataReader rdr = await cmd.ExecuteReaderAsync();

                while (rdr.Read())
                {
                    Tax entity = new Tax();
                    entity.ID = Convert.ToInt32(rdr["TaxID"]);
                    entity.Name = rdr["TaxName"].ToString() ?? "";
                    entity.Remarks = rdr["Remarks"].ToString() ?? "";
                    entity.Code = rdr["TaxCode"].ToString() ?? "";
                    entity.Rate = Convert.ToDecimal(rdr["Rate1"]);

                    listEntity.Add(entity);
                }
                _dbConnection.Close();
            }
            return listEntity;
        }

        public async Task<Tax> Get(int id)
        {

            Tax entity = new Tax();
            using (_dbConnection as SqlConnection)
            {
                SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();
                cmd.CommandText = "select * from TAX where CompanyID = 1 and TaxID = @Id";
                cmd.Parameters.AddWithValue("@id", id);

                cmd.CommandType = CommandType.Text;
                _dbConnection.Open();
                IDataReader rdr = await cmd.ExecuteReaderAsync();

                while (rdr.Read())
                {
                    entity.ID = Convert.ToInt32(rdr["TaxID"]);
                    entity.Name = rdr["TaxName"].ToString()??"";
                    entity.Remarks = rdr["Remarks"].ToString() ?? "";
                    entity.Code = rdr["TaxCode"].ToString() ?? "";
                    entity.Rate = rdr["Rate1"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["Rate1"]);

                }
                _dbConnection.Close();
            }
            return entity;
        }

    }
}

