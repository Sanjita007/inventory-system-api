using inventory_system_api.Application.IRepository;
using inventory_system_api.Application.IService;
using inventory_system_api.Application.Models.System;

namespace inventory_system_api.Infrastructure.Service
{
    public class UserService : IUserService
    {
        private IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> AddEdit(User entity, int userId, CancellationToken cancellationToken)
        {
            return await _repo.AddEdit(entity, userId, cancellationToken);
        }

        public async Task<int> Delete(int id, int userId, CancellationToken cancellationToken)
        {
            return await _repo.Delete(id, userId, cancellationToken);
        }

        public async Task<List<UserMin>> Get(CancellationToken cancellationToken)
        {
            return await _repo.Get(cancellationToken);
        }

        public async Task<UserMin?> Get(int id, CancellationToken cancellationToken)
        {
            return await _repo.Get(id, cancellationToken);
        }

        public async Task<int> UpdatePassword(UpdatePasswordModel passwordModel, int userId, CancellationToken cancellationToken)
        {
            return await _repo.UpdatePassword(passwordModel, userId, cancellationToken);
        }

        public async Task<bool> ValidatePassword(int userID, string password, CancellationToken cancellationToken)
        {
            return await _repo.ValidatePassword(userID, password, cancellationToken);
        }

        public async Task<User?> VerifyAndGetUserDetails(string userName, string password, CancellationToken cancellationToken)
        {
            return await _repo.VerifyAndGetUserDetails(userName, password, cancellationToken);
        }
    }
}
