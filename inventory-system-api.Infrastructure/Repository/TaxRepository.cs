using inventory_system_api.Application.IRepository;
using inventory_system_api.Application.Models.System;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;

namespace inventory_system_api.Infrastructure.Repository
{
    public class TaxRepository : ITaxRepository
    {
        IDbConnection _dbConnection;
        public TaxRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<int> AddEdit(Tax entity, CancellationToken cancellationToken, int userId)
        {
            using (_dbConnection as SqlConnection)
            {
                var parameters = new DynamicParameters(new
                {
                    entity.ID,
                    entity.Code,
                    entity.Name,
                    entity.Remarks,
                    entity.Rate,
                    userId
                });

                parameters.Add("return", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await _dbConnection.ExecuteAsync(new CommandDefinition("[SP_TAX_ADD_EDIT]", parameters, 
                    commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken));

                return parameters.Get<int>("return");
            }
        }

        public async Task<int> Delete(int id, CancellationToken cancellationToken, int userId)
        {

            using (_dbConnection as SqlConnection)
            {
                string commandText = "[SP_TAX_DELETE]";
               
                _dbConnection.Open();
                return await _dbConnection.ExecuteAsync(new CommandDefinition(commandText, new { id },
                    commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken));

            }
        }

        public async Task<List<Tax>> Get(CancellationToken cancellationToken)
        {

            List<Tax> listEntity = [];
            using (_dbConnection as SqlConnection)
            {
                string commandText = "SP_GET_TAX";
                _dbConnection.Open();

                return await _dbConnection.QueryAsync<Tax>(commandText, 
                    commandType: CommandType.StoredProcedure).ContinueWith(t => t.Result.ToList(), cancellationToken);
               
            }
        }

        public async Task<Tax?> Get(int id, CancellationToken cancellationToken)
        {

            Tax entity = new Tax();
            using (_dbConnection as SqlConnection)
            {
                string commandText = "SP_GET_TAX";
                _dbConnection.Open();
               
                return await _dbConnection.QueryFirstOrDefaultAsync<Tax>(new CommandDefinition(commandText, new { id }, 
                    commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken));
            }
        }

    }
}

