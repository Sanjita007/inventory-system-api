using inventory_system_api.IRepository.Invenetory;
using inventory_system_api.Models;
using inventory_system_api.Models.Inventory;
using Microsoft.Data.SqlClient;
using System.ComponentModel;
using System.Data;
using System.Data.Common;

namespace inventory_system_api.Repository.Inventory
{
    public class ProductGroupRepository : IProductGroupRepository
    {
        IDbConnection _dbConnection;
        public ProductGroupRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<int> AddEdit(ProductGroup entity)
        {
            int res = 0;
            using (_dbConnection as SqlConnection)
            {
                SqlParameter result = new SqlParameter("@return", dbType: SqlDbType.VarChar, 200);
                result.Direction = ParameterDirection.Output;

                SqlCommand cmd = _dbConnection.CreateCommand() as SqlCommand;
                cmd.CommandText = "[Inv].[SP_PRODUCT_GROUP_ADD_EDIT]";
                cmd.Parameters.AddWithValue("@id", entity.ID);
                cmd.Parameters.AddWithValue("@EngName", entity.EngName);
                cmd.Parameters.AddWithValue("@NepName", entity.NepName);
                cmd.Parameters.AddWithValue("@ParentGroupID", entity.ParentGroupID);
                cmd.Parameters.AddWithValue("@Remarks", entity.Remarks);
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
                cmd.CommandText = "[Inv].[spProductGroupDelete]";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.CommandType = CommandType.StoredProcedure;
                _dbConnection.Open();
                return await cmd.ExecuteNonQueryAsync();

            }
        }

        public async Task<List<ProductGroup>> Get()
        {

            List<ProductGroup> listEntity = [];
            using (_dbConnection as SqlConnection)
            {
                SqlCommand cmd = _dbConnection.CreateCommand() as SqlCommand;
                cmd.CommandText = $"select g.GroupID, g.Parent_GrpID ParentGroupID, g.EngName EngName, g.NepName, pg.EngName ParentGroupName, g.Level,g.Remarks " +
                    $"from INv.tblProductGroup g left join Inv.tblProductGroup pg on pg.GroupID = g.Parent_GrpID  where g.CompanyID =1";
                cmd.CommandType = CommandType.Text;
                _dbConnection.Open();
                IDataReader rdr = await cmd.ExecuteReaderAsync();

                while (rdr.Read())
                {
                    ProductGroup entity = new ProductGroup();
                    entity.ID = Convert.ToInt32(rdr["GroupID"]);
                    entity.ParentGroupID = rdr["ParentGroupID"] == DBNull.Value ? 0 : Convert.ToInt32(rdr["ParentGroupID"]);
                    entity.EngName = rdr["EngName"].ToString();
                    entity.NepName = rdr["NepName"].ToString();
                    entity.Level =  rdr["Level"] == DBNull.Value ? 0: Convert.ToInt32(rdr["Level"]);
                    entity.ParentGroupName = rdr["ParentGroupName"].ToString();
                    entity.Remarks = rdr["Remarks"].ToString();

                    listEntity.Add(entity);
                }
                _dbConnection.Close();
            }
            return listEntity;
        }

        public async Task<ProductGroup> Get(int id)
        {

            ProductGroup entity = null;
            using (_dbConnection as SqlConnection)
            {
                SqlCommand cmd = _dbConnection.CreateCommand() as SqlCommand;
                cmd.CommandText = $"select g.GroupID, g.Parent_GrpID ParentGroupID, g.EngName EngName, g.NepName, pg.EngName ParentGroupName, g.Level, g.Remarks " +
                    $"from INv.tblProductGroup g left join Inv.tblProductGroup pg on pg.GroupID = g.Parent_GrpID  where g.GroupID = @id and g.CompanyID =1";
                cmd.Parameters.AddWithValue("@id", id);

                cmd.CommandType = CommandType.Text;
                _dbConnection.Open();
                IDataReader rdr = await cmd.ExecuteReaderAsync();

                while (rdr.Read())
                {
                    entity = new ProductGroup
                    {
                        ID = Convert.ToInt32(rdr["GroupID"]),
                        ParentGroupID = rdr["ParentGroupID"] == DBNull.Value ? 0 : Convert.ToInt32(rdr["ParentGroupID"]),
                        EngName = rdr["EngName"].ToString(),
                        NepName = rdr["NepName"].ToString(),
                        Level = rdr["Level"] == DBNull.Value ? 0 : Convert.ToInt32(rdr["Level"]),
                        ParentGroupName = rdr["ParentGroupName"].ToString(),
                        Remarks = rdr["Remarks"].ToString()

                    };

                }
                _dbConnection.Close();
            }
            return entity;
        }

        public async Task<List<Tree>> GetProductTrees()
        {
            List<ProductGroup> list = await Get();
            List<Tree> tree = TreeMethod(list, 0, 0);

            return tree;
        }

        public List<Tree> TreeMethod(List<ProductGroup> group, int level, int id)
        {
            List<Tree> children = [];

            //this is for the root, get the information of the root and continue with the children as usual
            if(id == 0 && level == 0)
            {
                // there is just 1 root and others are its children, so select only 1
                ProductGroup gs = group.Where(r => r.Level == 0).FirstOrDefault();
                
                children.Add(new Tree() { Id = gs.ID, Level = 0, Name = gs.EngName, ParentID = 0 });

                // set id to the id of the root, so in the next few steps the loop can find its children one by one
                id = gs.ID;
            }

            List<ProductGroup> list = [.. group.Where(r => r.ParentGroupID == id)];

            foreach (ProductGroup l in list)
            {
                Tree t = new()
                {
                    Id = l.ID,
                    ParentID = l.ParentGroupID,
                    Level = level + 1,
                    Name = l.EngName,
                    Children = TreeMethod(group, level + 1, l.ID)
                };
                children.Add(t);
            }

            return children;
        }

    }
}

