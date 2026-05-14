using inventory_system_api.Application.Models.System;

namespace inventory_system_api.Application.IRepository
{
    public interface IUserRepository
    {
        
        public Task<List<User>> Get(CancellationToken cancellationToken);

        public Task<User> Get(int id, CancellationToken cancellationToken);

        public Task<int> AddEdit(User entity, CancellationToken cancellationToken);
        public Task<int> Delete(int id, CancellationToken cancellationToken);
       
        public Task<User> VerifyAndGetUserDetails(string userName, string password, CancellationToken cancellationToken);
        public Task<bool> ValidatePassword(int userID, string password, CancellationToken cancellationToken);

        public Task<int> UpdatePassword(UpdatePasswordModel passwordModel, CancellationToken cancellationToken);




    }
}
