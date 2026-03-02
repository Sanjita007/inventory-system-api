using inventory_system_api.Application.IRepository.Invenetory;
using inventory_system_api.Models.Inventory;
using inventory_system_api.Shared;
using Microsoft.Data.SqlClient;
using System.Data;

namespace inventory_system_api.Infrastructure.Repository.Inventory
{
    public class ProductRepository : IProductRepository
    {
        IDbConnection _dbConnection;
        public ProductRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<int> AddEdit(Product entity)
        {
            int res = 0;
            using (_dbConnection as SqlConnection)
            {
                SqlParameter result = new SqlParameter("@return", dbType: SqlDbType.VarChar, 200);
                result.Direction = ParameterDirection.Output;

                SqlCommand cmd = _dbConnection.CreateCommand() as SqlCommand;
                cmd.CommandText = "[Inv].[SP_PRODUCT_ADD_EDIT]";
                cmd.Parameters.AddWithValue("@id", entity.ID);
                cmd.Parameters.AddWithValue("@EngName", entity.EngName);
                cmd.Parameters.AddWithValue("@NepName", entity.NepName);
                cmd.Parameters.AddWithValue("@GroupID", entity.GroupID);
                cmd.Parameters.AddWithValue("@Code", entity.Code);
                cmd.Parameters.AddWithValue("@Color", entity.BackColor);
                cmd.Parameters.AddWithValue("@UnitID", entity.UnitID);
                cmd.Parameters.AddWithValue("@IsVatApplicable", entity.IsVatApplicable);
                cmd.Parameters.AddWithValue("@IsInventoryApplicable", entity.IsInventoryApplicable);
                cmd.Parameters.AddWithValue("@IsDecimalApplicable", entity.IsDecimalApplicable);
                cmd.Parameters.AddWithValue("@IsActive", entity.IsActive);
                cmd.Parameters.AddWithValue("@CompanyID", entity.CompanyID);
                cmd.Parameters.AddWithValue("@Image", entity.Image?.FromBase64());
                cmd.Parameters.AddWithValue("@Remarks", entity.Remarks);

                cmd.Parameters.AddWithValue("@OpenPurchaseRate", entity.PurchaseRate);
                cmd.Parameters.AddWithValue("@OpenSalesRate", entity.SalesRate);
                cmd.Parameters.AddWithValue("@TaxID", entity.TaxID);
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
                SqlCommand cmd = _dbConnection.CreateCommand() as SqlCommand;
                cmd.CommandText = "[Inv].[spProductDelete]";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.CommandType = CommandType.StoredProcedure;
                _dbConnection.Open();
                return await cmd.ExecuteNonQueryAsync();

            }
        }

        public async Task<List<Product>> Get()
        {

            List<Product> listEntity = [];
            using (_dbConnection as SqlConnection)
            {
                SqlCommand cmd = _dbConnection.CreateCommand() as SqlCommand;
                cmd.CommandText = "select p.*, u.UnitName, u.Symbol From inv.tblProduct p inner join System.tblUnitMaintenance u on p.unitMaintenanceID = u.unitMaintenanceID where p.CompanyID = '1'";
                cmd.CommandType = CommandType.Text;
                _dbConnection.Open();
                IDataReader rdr = await cmd.ExecuteReaderAsync();

                while (rdr.Read())
                {
                    Product product = new Product
                    {
                        ID = Convert.ToInt32(rdr["ProductID"]),
                        Code = rdr["ProductCode"].ToString(),
                        EngName = rdr["EngName"].ToString(),
                        NepName = rdr["NepName"].ToString(),
                        GroupID = rdr["GroupID"] == DBNull.Value ? 0 : Convert.ToInt32(rdr["GroupID"]),
                        SalesRate = rdr["SalesRate"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["SalesRate"]),
                        PurchaseRate = rdr["PurchaseRate"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["PurchaseRate"]),
                        TaxID = rdr["TaxID"] == DBNull.Value ? null : Convert.ToInt32(rdr["TaxID"]),
                        IsActive = rdr["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(rdr["IsActive"]),
                        IsInventoryApplicable = rdr["IsINventoryApplicable"] == DBNull.Value ? false : Convert.ToBoolean(rdr["IsINventoryApplicable"]),
                        IsDecimalApplicable = rdr["IsDecimalApplicable"] == DBNull.Value ? false : Convert.ToBoolean(rdr["IsDecimalApplicable"]),
                        IsVatApplicable = rdr["IsVatApplicable"] == DBNull.Value ? false : Convert.ToBoolean(rdr["IsVatApplicable"]),
                        IsBuiltIn = rdr["BuiltIn"] == DBNull.Value ? false : Convert.ToBoolean(rdr["BuiltIn"]),
                        UnitID = rdr["UnitMaintenanceID"] == DBNull.Value ? 0 : Convert.ToInt32(rdr["UnitMaintenanceID"]),
                        UnitName = rdr["UnitName"].ToString(),
                        UnitSymbol = rdr["Symbol"].ToString(),
                        CreatedBy = rdr["Created_By"].ToString() ?? "",
                        CreatedDate = rdr["Created_Date"] == DBNull.Value ? null : Convert.ToDateTime(rdr["Created_Date"]),
                        Image = rdr["Image"] == DBNull.Value ? null : ((byte[])rdr["Image"]).ToBase64()

                    };

                    listEntity.Add(product);
                }
                _dbConnection.Close();
            }
            return listEntity;
        }

        public async Task<Product> Get(int id)
        {

            Product entity = null;
            using (_dbConnection as SqlConnection)
            {
                SqlCommand cmd = _dbConnection.CreateCommand() as SqlCommand;
                cmd.CommandText = "select * from Inv.tblProduct where ProductID = @Id and CompanyID =1";
                cmd.Parameters.AddWithValue("@id", id);

                cmd.CommandType = CommandType.Text;
                _dbConnection.Open();
                IDataReader rdr = await cmd.ExecuteReaderAsync();

                while (rdr.Read())
                {
                    entity = new Product
                    {
                        ID = Convert.ToInt32(rdr["ProductID"]),
                        Code = rdr["ProductID"].ToString(),
                        EngName = rdr["EngName"].ToString(),
                        NepName = rdr["NepName"].ToString(),
                        GroupID = Convert.ToInt32(rdr["GroupID"]),
                        SalesRate = rdr["SalesRate"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["SalesRate"]),
                        PurchaseRate = rdr["PurchaseRate"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["PurchaseRate"]),
                        TaxID = rdr["TaxID"] == DBNull.Value ? null : Convert.ToInt32(rdr["TaxID"]),
                        IsActive = rdr["IsActive"] != DBNull.Value & rdr["IsActive"] != "0",
                        IsInventoryApplicable = rdr["IsINventoryApplicable"] != DBNull.Value & rdr["IsINventoryApplicable"].ToString() != "0",
                        IsDecimalApplicable = rdr["IsDecimalApplicable"] != DBNull.Value & rdr["IsDecimalApplicable"].ToString() != "0",
                        IsVatApplicable = rdr["IsVatApplicable"] != DBNull.Value & rdr["IsVatApplicable"].ToString() != "0",
                        IsBuiltIn = rdr["BuiltIn"] != DBNull.Value & rdr["BuiltIn"].ToString() != "0",
                        UnitID = Convert.ToInt32(rdr["UnitMaintenanceID"]),
                        CreatedBy = rdr["Created_By"].ToString(),
                        CreatedDate = Convert.ToDateTime(rdr["Created_Date"]),
                        Image = rdr["Image"] == DBNull.Value ? null : ((byte[])rdr["Image"]).ToBase64()

                    };

                }
                _dbConnection.Close();
            }
            return entity;
        }

        public async Task<Product> Search(string code)
        {
            Product entity = null;
            using (_dbConnection as SqlConnection)
            {
                SqlCommand cmd = _dbConnection.CreateCommand() as SqlCommand;
                cmd.CommandText = "select * from Inv.tblProduct where ProductCode = @code and CompanyID =1";
                cmd.Parameters.AddWithValue("@code", code);

                cmd.CommandType = CommandType.Text;
                _dbConnection.Open();
                IDataReader rdr = await cmd.ExecuteReaderAsync();

                while (rdr.Read())
                {
                    entity = new Product
                    {
                        ID = Convert.ToInt32(rdr["ProductID"]),
                        Code = rdr["ProductID"].ToString(),
                        EngName = rdr["EngName"].ToString(),
                        NepName = rdr["NepName"].ToString(),
                        Email = rdr["Email"].ToString(),
                        SalesRate = rdr["SalesRate"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["SalesRate"]),
                        PurchaseRate = rdr["PurchaseRate"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["PurchaseRate"]),
                        DepotID = Convert.ToInt32(rdr["DepotID"]),
                        TaxID = rdr["TaxID"] == DBNull.Value ? null : Convert.ToInt32(rdr["TaxID"]),
                        IsActive = rdr["IsActive"] != DBNull.Value & rdr["IsActive"] != "0",
                        IsInventoryApplicable = rdr["IsINventoryApplicable"] != DBNull.Value & rdr["IsINventoryApplicable"].ToString() != "0",
                        IsDecimalApplicable = rdr["IsDecimalApplicable"] != DBNull.Value & rdr["IsDecimalApplicable"].ToString() != "0",
                        IsVatApplicable = rdr["IsVatApplicable"] != DBNull.Value & rdr["IsVatApplicable"].ToString() != "0",
                        IsBuiltIn = rdr["BuiltIn"] != DBNull.Value & rdr["BuiltIn"].ToString() != "0",
                        UnitID = Convert.ToInt32(rdr["UnitMaintenanceID"]),
                        CreatedBy = rdr["Created_By"].ToString(),
                        CreatedDate = Convert.ToDateTime(rdr["Created_Date"]),
                        Image = rdr["Image"] == DBNull.Value ? null : ((byte[])rdr["Image"]).ToBase64()

                    };

                }
                _dbConnection.Close();
            }
            return entity;
        }

    }
}

