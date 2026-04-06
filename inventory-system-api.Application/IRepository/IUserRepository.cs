using inventory_system_api.Application.Models.System;

namespace inventory_system_api.Application.IRepository
{
    public interface IUserRepository
    {
        
        public Task<List<User>> Get();

        public Task<User> Get(int id);

        public Task<int> AddEdit(User entity);

        public Task<int> Delete(int id);
       
        public Task<User> VerifyAndGetUserDetails(string userName, string password);
        public Task<bool> ValidatePassword(int userID, string password);

        public Task<int> UpdatePassword(UpdatePasswordModel passwordModel);




    }
}
