using Dapper;
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

        public async Task<int> AddEdit(CompoundUnit entity, int userId, CancellationToken cancellationToken)
        {
            using (_dbConnection as SqlConnection)
            {
                var parameters = new DynamicParameters(new
                {
                    entity.ID,
                    entity.UnitID,
                    entity.ParentUnitID,
                    entity.RelationValue,
                    entity.Remarks,
                    userId
                });

                parameters.Add("return", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await _dbConnection.ExecuteAsync(new CommandDefinition("[SP_COMPOUND_UNIT_ADD_EDIT]", parameters,
                    commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken));

                return parameters.Get<int>("return");
            }
        }

        public async Task<decimal?> ConvertUnit(int defaultUnitID, int currentUnitID, decimal valueToConvert, CancellationToken cancellationToken)
        {
            using (_dbConnection as SqlConnection)
            {
                using SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();

                string commandText = "SP_CONVERT_COMPOUND_UNIT";

                var parameters = new DynamicParameters(
                new
                {
                    defUnitID = defaultUnitID,
                    currUnitID = currentUnitID,
                    actualValue = valueToConvert
                });


                _dbConnection.Open();
                return await _dbConnection.ExecuteScalarAsync<decimal>(new CommandDefinition(commandText, parameters,
                    commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken));
            }
        }

        public async Task<int> Delete(int id, int userId, CancellationToken cancellationToken)
        {
            using (_dbConnection as SqlConnection)
            {
                string commandText = "[SP_COMPOUND_UNIT_DELETE]";

                _dbConnection.Open();
                return await _dbConnection.ExecuteAsync(new CommandDefinition(commandText, new { id },
                    commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken));

            }
        }

        public async Task<List<CompoundUnit>> Get(CancellationToken cancellationToken)
        {
            using (_dbConnection as SqlConnection)
            {
                string commandText = "SP_GET_COMPOUND_UNIT";
                _dbConnection.Open();

                return await _dbConnection.QueryAsync<CompoundUnit>(commandText,
                    commandType: CommandType.StoredProcedure).ContinueWith(t => t.Result.ToList(), cancellationToken);

            }
        }

        public async Task<CompoundUnit?> Get(int id, CancellationToken cancellationToken)
        {

            using (_dbConnection as SqlConnection)
            {
                string commandText = "SP_GET_COMPOUND_UNIT";
                _dbConnection.Open();

                return await _dbConnection.QueryFirstOrDefaultAsync<CompoundUnit>(new CommandDefinition(commandText, new { id },
                    commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken));

            }
        }

        public async Task<List<UnitDetails>> GetRelatedUnit(int BaseUnitID, CancellationToken cancellationToken)
        {
            using (_dbConnection as SqlConnection)
            {
                string commandText = "SP_GET_UNIT_CONVERSION_RATES";
                _dbConnection.Open();

                return await _dbConnection.QueryAsync<UnitDetails>(commandText, new { BaseUnitID },
                    commandType: CommandType.StoredProcedure).ContinueWith(t => t.Result.ToList(), cancellationToken);

            }
        }

        public async Task<List<UnitDetails>> GetMultipleRelatedUnit(string baseUnits, CancellationToken cancellationToken)
        {
            using (_dbConnection as SqlConnection)
            {
                string commandText = "SP_GET_MULTIPLE_UNIT_CONVERSION_RATES";
                _dbConnection.Open();

                return await _dbConnection.QueryAsync<UnitDetails>(commandText, new { UNITSCSV = baseUnits },
                    commandType: CommandType.StoredProcedure).ContinueWith(t => t.Result.ToList(), cancellationToken);

            }
        }

    }

}
