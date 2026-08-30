using inventory_system_api.Application.Models.System;

namespace inventory_system_api.Application.IService
{
    public interface IUserService
    {
        public Task<List<UserMin>> Get(CancellationToken cancellationToken);

        public Task<UserMin?> Get(int id, CancellationToken cancellationToken);

        public Task<int> AddEdit(User entity, int userId, CancellationToken cancellationToken);
        public Task<int> Delete(int id, int userId, CancellationToken cancellationToken);

        public Task<User?> VerifyAndGetUserDetails(string userName, string password, CancellationToken cancellationToken);
        public Task<bool> ValidatePassword(int userID, string password, CancellationToken cancellationToken);

        public Task<int> UpdatePassword(UpdatePasswordModel passwordModel, int userId, CancellationToken cancellationToken);


    }
}
