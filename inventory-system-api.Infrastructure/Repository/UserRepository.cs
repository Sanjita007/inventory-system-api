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

        public async Task<int> AddEdit(User entity, CancellationToken cancellationToken)
        {
            int res = 0;

            using (_dbConnection as SqlConnection)
            {
                using SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();
                cmd.CommandText = "[SP_USER_ADD_EDIT]";
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter result = new SqlParameter("@return", dbType: SqlDbType.VarChar, 200);
                result.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(result);

                cmd.Parameters.AddWithValue("@id", entity.ID);
                cmd.Parameters.AddWithValue("@UserName", entity.UserName);
                if(entity.ID <= 0)
                    cmd.Parameters.AddWithValue("@Password", Utility.HashPassword(entity.Password));
                cmd.Parameters.AddWithValue("@Name", entity.Name);
                cmd.Parameters.AddWithValue("@Address", entity.Address);
                cmd.Parameters.AddWithValue("@Contact", entity.PhoneNo);
                cmd.Parameters.AddWithValue("@Email", entity.Email);

                cmd.Parameters.AddWithValue("@UserID", "root");

                _dbConnection.Open();
                await cmd.ExecuteNonQueryAsync(cancellationToken);
                res = Convert.ToInt32(result.Value);

            }
            return res;
        }

        public async Task<int> Delete(int id, CancellationToken cancellationToken)
        {
            using (_dbConnection as SqlConnection)
            {
                using SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();

                cmd.CommandText = "[SP_USER_DELETE]";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);

                _dbConnection.Open();
                return await cmd.ExecuteNonQueryAsync(cancellationToken);
            }
        }

        public async Task<List<User>> Get(CancellationToken cancellationToken)
        {
            List<User> listEntity = new List<User>();

            using (_dbConnection as SqlConnection)
            {
                using SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();

                cmd.CommandText = "select UserID, UserName, Name, Address, Contact, Email,Department, Role from [User] where CompanyID = 1";
                cmd.CommandType = CommandType.Text;

                _dbConnection.Open();

                using IDataReader rdr = await cmd.ExecuteReaderAsync(cancellationToken);
                while (rdr.Read())
                {
                    listEntity.Add(new User
                    {
                        ID = Convert.ToInt32(rdr["UserID"]),
                        Name = rdr["Name"].ToString()??"",
                        UserName = rdr["UserName"].ToString() ?? "",
                        Address = rdr["Address"].ToString() ?? "",
                        PhoneNo = rdr["Contact"].ToString() ?? "",
                        Email = rdr["Email"].ToString() ?? "",
                        Role = rdr["Role"].ToString() ?? "",

                    });
                }
            }
            return listEntity;
        }

        public async Task<User> Get(int id, CancellationToken cancellationToken)
        {
            User entity = new();

            using (_dbConnection as SqlConnection)
            {
                using SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();

                cmd.CommandText = "select UserID, UserName, Name, Address, Contact, Email, Department, Role from [User] where CompanyID = 1 and UserID = @id";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@id", id);

                _dbConnection.Open();

                using IDataReader rdr = await cmd.ExecuteReaderAsync(cancellationToken);
                while (rdr.Read())
                {
                    entity = new User
                    {
                        ID = Convert.ToInt32(rdr["UserID"]),
                        Name = rdr["Name"].ToString()??"",
                        UserName = rdr["UserName"].ToString() ?? "",
                        Address = rdr["Address"].ToString() ?? "",
                        PhoneNo = rdr["Contact"].ToString() ?? "",
                        Email = rdr["Email"].ToString() ?? "",
                        Role = rdr["Role"].ToString() ?? "",
                    };
                }
            }
            return entity;
        }


        public async Task<User> VerifyAndGetUserDetails(string userName, string password, CancellationToken cancellationToken)
        {
            string pass = "";
            User user = new();

            using (_dbConnection as SqlConnection)
            {
                using SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();

                cmd.CommandText = "select * from [User] where CompanyID = 1 and UserName = @userName";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@userName", userName);

                _dbConnection.Open();

                using IDataReader rdr = await cmd.ExecuteReaderAsync(cancellationToken);
                while (rdr.Read())
                {
                    user = new User
                    {
                        ID = Convert.ToInt32(rdr["UserID"]),
                        Name = rdr["Name"].ToString() ?? "",
                        UserName = rdr["UserName"].ToString() ?? "",
                        Address = rdr["Address"].ToString() ?? "",
                        PhoneNo = rdr["Contact"].ToString() ?? "",
                        Email = rdr["Email"].ToString() ?? "",
                        Role = rdr["Role"].ToString() ?? "",
                    };

                    pass = rdr["Password"].ToString()??"";
                }
            }

            if (BCrypt.Net.BCrypt.Verify(password, pass))
            {
                return user;

            }
            else
            {
                return null;
            }
        }

        public async Task<bool> ValidatePassword(int userID, string password, CancellationToken cancellationToken)
        {
            string pass = "";

            using (_dbConnection as SqlConnection)
            {
                using SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();

                cmd.CommandText = "select Password from [User] where CompanyID = 1 and UserID = @id";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@id", userID);

                _dbConnection.Open();

                using IDataReader rdr = await cmd.ExecuteReaderAsync(cancellationToken);
                while (rdr.Read())
                {

                    pass = rdr["Password"].ToString()??"";
                }
            }

            return BCrypt.Net.BCrypt.Verify(password, pass);

        }

        public async Task<int> UpdatePassword(UpdatePasswordModel entity, CancellationToken cancellationToken)
        {
            int res = 0;

            using (_dbConnection as SqlConnection)
            {
                using SqlCommand cmd = (SqlCommand)_dbConnection.CreateCommand();
                cmd.CommandText = "[SP_PASSWORD_UPDATE]";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id", entity.ID);
                //cmd.Parameters.AddWithValue("@OldPassword", Utility.HashPassword(entity.OldPassword));
                cmd.Parameters.AddWithValue("@NewPassword", Utility.HashPassword(entity.NewPassword));

                //cmd.Parameters.AddWithValue("@UserID", "root");

                _dbConnection.Open();
                res = await cmd.ExecuteNonQueryAsync(cancellationToken);

            }
            return res;
        }
    }
}
