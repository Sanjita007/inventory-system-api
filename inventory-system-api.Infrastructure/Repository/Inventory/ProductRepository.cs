using Dapper;
using inventory_system_api.Application.IRepository.Invenetory;
using inventory_system_api.Models.Inventory;
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

        public async Task<int> AddEdit(Product entity, CancellationToken cancellationToken, int userId)
        {
            using (_dbConnection as SqlConnection)
            {
                var parameters = new DynamicParameters(new
                {
                    entity.ID,
                    entity.EngName,
                    entity.NepName,
                    entity.GroupID,
                    entity.Code,
                    entity.UnitID,
                    entity.SalesRate,
                    entity.OpeningQuantity,
                    entity.PurchaseRate,

                    entity.IsVatApplicable,
                    entity.IsInventoryApplicable,
                    entity.IsDecimalApplicable,
                    entity.IsActive,
                    entity.CompanyID,
                    entity.Remarks,
                    entity.BackColor,

                    entity.ParentProductID,
                    entity.Size,

                    // CONTACT & COMPANY DETAILS
                    entity.ContactPerson,
                    entity.Address1,
                    entity.Address2,
                    entity.City,
                    entity.Telephone,
                    entity.Email,
                    entity.Company,
                    entity.Website,

                    
                    entity.TaxID,
                    userId
                });

                parameters.Add("return", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await _dbConnection.ExecuteAsync(new CommandDefinition("SP_PRODUCT_ADD_EDIT", parameters,
                    commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken));

                return parameters.Get<int>("return");
            }            
        }

        public async Task<int> Delete(int id, CancellationToken cancellationToken, int userId)
        {
            using (_dbConnection as SqlConnection)
            {
                string commandText = "SP_PRODUCT_DELETE";

                _dbConnection.Open();
                return await _dbConnection.ExecuteAsync(new CommandDefinition(commandText, new { id },
                    commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken));

            }
        }

        public async Task<List<Product>> Get(CancellationToken cancellationToken)
        {
            using (_dbConnection as SqlConnection)
            {
                string commandText = "SP_GET_PRODUCT";
                _dbConnection.Open();

                return await _dbConnection.QueryAsync<Product>(commandText,
                    commandType: CommandType.StoredProcedure).ContinueWith(t => t.Result.ToList(), cancellationToken);

            }
        }

        public async Task<Product?> Get(int id, CancellationToken cancellationToken)
        {
            using (_dbConnection as SqlConnection)
            {
                string commandText = "SP_GET_PRODUCT";
                _dbConnection.Open();

                return await _dbConnection.QueryFirstOrDefaultAsync<Product>(new CommandDefinition(commandText, new { id },
                    commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken));

            }

        }

        public async Task<List<Product>> Search(string code, CancellationToken cancellationToken)
        {
            using (_dbConnection as SqlConnection)
            {
                string commandText = "SP_GET_PRODUCT";
                _dbConnection.Open();

                return await _dbConnection.QueryAsync<Product>(commandText, new { SEARCH_VALUE = code },
                    commandType: CommandType.StoredProcedure).ContinueWith(t => t.Result.ToList(), cancellationToken);

            }

        }
    }
}

