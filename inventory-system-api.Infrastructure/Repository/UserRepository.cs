using Dapper;
using inventory_system_api.Application.IRepository;
using inventory_system_api.Application.Models.System;
using inventory_system_api.Shared;
using Microsoft.Data.SqlClient;
using System.Data;

namespace inventory_system_api.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        IDbConnection _dbConnection;
        public UserRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<int> AddEdit(User entity, int userId, CancellationToken cancellationToken)
        {
            // udpate the password field with the hash value
            entity.Password = Utility.HashPassword(entity.Password);

            // removing the using statement for now because an error is showing "The ConnectionString property has not been initialized."
            //using (_dbConnection as SqlConnection)
            //{
            var parameters = new DynamicParameters(new
            {
                entity.ID,
                entity.UserName,
                entity.Password,
                entity.PhoneNo,
                entity.Email,
                entity.Address,
                entity.Name,
                entity.Role
            });

            parameters.Add("return", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("userID", userId, dbType: DbType.Int32);

            await _dbConnection.ExecuteAsync(new CommandDefinition("[SP_USER_ADD_EDIT]", parameters,
                commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken));

            return parameters.Get<int>("return");

            // }

        }

        public async Task<int> Delete(int id, int userId, CancellationToken cancellationToken)
        {
            using (_dbConnection as SqlConnection)
            {
                string commandText = "[SP_USER_DELETE]";

                _dbConnection.Open();
                return await _dbConnection.ExecuteAsync(new CommandDefinition(commandText, new { id },
                    commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken));

            }
        }

        public async Task<List<UserMin>> Get(CancellationToken cancellationToken)
        {

            using (_dbConnection as SqlConnection)
            {
                string commandText = "SP_GET_USER";
                _dbConnection.Open();

                return await _dbConnection.QueryAsync<UserMin>(commandText,
                    commandType: CommandType.StoredProcedure).ContinueWith(t => t.Result.ToList(), cancellationToken);

            }
        }

        public async Task<UserMin?> Get(int id, CancellationToken cancellationToken)
        {

            using (_dbConnection as SqlConnection)
            {
                string commandText = "SP_GET_USER";
                _dbConnection.Open();

                return await _dbConnection.QueryFirstOrDefaultAsync<UserMin>(new CommandDefinition(commandText, new { id },
                    commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken));

            }
        }


        public async Task<User?> VerifyAndGetUserDetails(string userName, string password, CancellationToken cancellationToken)
        {
            User? user = new();

            using (_dbConnection as SqlConnection)
            {
                string commandText = "SP_GET_USER";
                _dbConnection.Open();

                user = await _dbConnection.QueryFirstOrDefaultAsync<User>(new CommandDefinition(commandText, new { userName },
                    commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken));

            }

            if (BCrypt.Net.BCrypt.Verify(password, user?.Password))
            {
                return user;

            }
            else
            {
                return null;
            }
        }

        public async Task<bool> ValidatePassword(int id, string password, CancellationToken cancellationToken)
        {
            string pass = "";

            //using (_dbConnection as SqlConnection)
            //{
            using SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();

            string commandText = "SP_GET_PASSWORD_BY_USERID";

            _dbConnection.Open();

            pass = await _dbConnection.ExecuteScalarAsync<string>(new CommandDefinition(commandText, new { id },
                commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)) ?? "";

            //}

            return BCrypt.Net.BCrypt.Verify(password, pass);

        }

        public async Task<int> UpdatePassword(UpdatePasswordModel entity, int userId, CancellationToken cancellationToken)
        {
            using (_dbConnection as SqlConnection)
            {

                var parameters = new DynamicParameters(new
                {
                    entity.ID,
                    NewPassword = Utility.HashPassword(entity.NewPassword)
                });

                return await _dbConnection.ExecuteAsync(new CommandDefinition("[SP_PASSWORD_UPDATE]", parameters,
                    commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken));

            }
        }
    }
}
