using inventory_system_api.Application.IRepository.Invenetory;
using inventory_system_api.Application.Models.Inventory;
using Microsoft.Data.SqlClient;
using System.Data;

namespace inventory_system_api.Infrastructure.Repository.Inventory
{
    public class DepotRepository : IDepotRepository
    {
        IDbConnection _dbConnection;
        public DepotRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<int> AddEdit(Depot entity)
        {
            int res = 0;
            //using (_dbConnection as SqlConnection)
            //{
            //    SqlParameter result = new SqlParameter("@return", dbType: SqlDbType.VarChar, 200);
            //    result.Direction = ParameterDirection.Output;
                
            //    SqlCommand cmd = _dbConnection.CreateCommand() as SqlCommand;
            //    cmd.CommandText = "[Inv].[spProductAddEdit]";
            //    cmd.Parameters.AddWithValue("@id", entity.ID);
            //    cmd.Parameters.AddWithValue("@EngName", entity.EngName);
            //    cmd.Parameters.AddWithValue("@NepName", entity.NepName);
            //    cmd.Parameters.AddWithValue("@GroupID", entity.GroupID);
            //    cmd.Parameters.AddWithValue("@Code", entity.Code);
            //    cmd.Parameters.AddWithValue("@Color", entity.BackColor);
            //    cmd.Parameters.AddWithValue("@DepotID", entity.DepotID);
            //    cmd.Parameters.AddWithValue("@UnitID", entity.UnitID);
            //    cmd.Parameters.AddWithValue("@IsVatApplicable", entity.IsVatApplicable);
            //    cmd.Parameters.AddWithValue("@IsActive", entity.IsActive);
            //    cmd.Parameters.AddWithValue("@CompanyID", entity.CompanyID);
            //    cmd.Parameters.AddWithValue("@Size", entity.Size);
            //    cmd.Parameters.AddWithValue("@OpenPurchaseQty", entity.PurchaseQuantity);
            //    cmd.Parameters.AddWithValue("@PurchaseRate", entity.PurchaseRate);
            //    cmd.Parameters.AddWithValue("@TaxID", entity.TaxID);
            //    cmd.Parameters.AddWithValue("@UserID", "root");
            //    cmd.Parameters.Add(result);

            //    cmd.CommandType = CommandType.StoredProcedure;
            //    _dbConnection.Open();
            //    res = await cmd.ExecuteNonQueryAsync();

            //}

            return res;
        }

        public async Task<int> Delete(int id)
        {

            using (_dbConnection as SqlConnection)
            {
                SqlCommand cmd = _dbConnection.CreateCommand() as SqlCommand;
                cmd.CommandText = "[Inv].[spDepotDelete]";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.CommandType = CommandType.StoredProcedure;
                _dbConnection.Open();
                return await cmd.ExecuteNonQueryAsync();

            }
        }

        public async Task<List<Depot>> Get()
        {

            List<Depot> listEntity = [];
            using (_dbConnection as SqlConnection)
            {
                SqlCommand cmd = _dbConnection.CreateCommand() as SqlCommand;
                cmd.CommandText = "select top 10 * from Inv.tblDepot where CompanyID = 1";
                cmd.CommandType = CommandType.Text;
                _dbConnection.Open();
                IDataReader rdr = await cmd.ExecuteReaderAsync();

                while (rdr.Read())
                {
                    Depot entity = new Depot();
                    entity.ID = Convert.ToInt32(rdr["DepotID"]);
                    entity.Name = rdr["DepotName"].ToString();
                   
                    listEntity.Add(entity);
                }
                _dbConnection.Close();
            }
            return listEntity;
        }

        public async Task<Depot> Get(int id)
        {

            Depot entity = null;
            using (_dbConnection as SqlConnection)
            {
                SqlCommand cmd = _dbConnection.CreateCommand() as SqlCommand;
                cmd.CommandText = "select * from Inv.tblDepot where DepotID = @Id and CompanyID =1";
                cmd.Parameters.AddWithValue("@id", id);

                cmd.CommandType = CommandType.Text;
                _dbConnection.Open();
                IDataReader rdr = await cmd.ExecuteReaderAsync();

                while (rdr.Read())
                {
                    entity.ID = Convert.ToInt32(rdr["DepotID"]);
                    entity.Name = rdr["DepotName"].ToString();

                }
                _dbConnection.Close();
            }
            return entity;
        }

    }
}

