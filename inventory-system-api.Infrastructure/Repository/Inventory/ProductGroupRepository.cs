using Dapper;
using inventory_system_api.Application.IRepository.Invenetory;
using inventory_system_api.Application.Models;
using inventory_system_api.Application.Models.Inventory;
using Microsoft.Data.SqlClient;
using System.Data;

namespace inventory_system_api.Infrastructure.Repository.Inventory
{
    public class ProductGroupRepository : IProductGroupRepository
    {
        IDbConnection _dbConnection;
        public ProductGroupRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<int> AddEdit(ProductGroup entity, int userId, CancellationToken cancellationToken)
        {
            using (_dbConnection as SqlConnection)
            {
                var parameters = new DynamicParameters(new
                {
                    entity.ID,
                    entity.EngName,
                    entity.NepName,
                    entity.ParentGroupID,
                    entity.Remarks,
                    userId
                });

                parameters.Add("return", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await _dbConnection.ExecuteAsync(new CommandDefinition("[SP_PRODUCT_GROUP_ADD_EDIT]", parameters,
                    commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken));

                return parameters.Get<int>("return");
            }

        }

        public async Task<int> Delete(int id, int userId, CancellationToken cancellationToken)
        {
            using (_dbConnection as SqlConnection)
            {
                string commandText = "SP_PRODUCT_GROUP_DELETE";

                _dbConnection.Open();
                return await _dbConnection.ExecuteAsync(new CommandDefinition(commandText, new { id },
                    commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken));

            }
        }

        public async Task<List<ProductGroup>> Get(CancellationToken cancellationToken)
        {
            using (_dbConnection as SqlConnection)
            {
                string commandText = "SP_GET_PRODUCT_GROUP";
                _dbConnection.Open();

                return await _dbConnection.QueryAsync<ProductGroup>(commandText,
                    commandType: CommandType.StoredProcedure).ContinueWith(t => t.Result.ToList(), cancellationToken);

            }

          }

        public async Task<ProductGroup?> Get(int id, CancellationToken cancellationToken)
        {

            using (_dbConnection as SqlConnection)
            {
                string commandText = "SP_GET_PRODUCT_GROUP";
                _dbConnection.Open();

                return await _dbConnection.QueryFirstOrDefaultAsync<ProductGroup>(new CommandDefinition(commandText, new { id },
                    commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken));

            }
            
        }

        public async Task<List<Tree>> GetProductTrees(CancellationToken cancellationToken)
        {
            List<ProductGroup> list = await Get(cancellationToken);
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
                ProductGroup? gs = group.FirstOrDefault(r => r.Level == 0);
                
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

