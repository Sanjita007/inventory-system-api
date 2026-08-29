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
        public async Task<int> AddEdit(User entity, CancellationToken cancellationToken, int userId)
        {
            return await _repo.AddEdit(entity, cancellationToken, userId);
        }

        public async Task<int> Delete(int id, CancellationToken cancellationToken, int userId)
        {
            return await _repo.Delete(id, cancellationToken, userId);
        }

        public async Task<List<UserMin>> Get(CancellationToken cancellationToken)
        {
            return await _repo.Get(cancellationToken);
        }

        public async Task<UserMin?> Get(int id, CancellationToken cancellationToken)
        {
            return await _repo.Get(id, cancellationToken);
        }

        public async Task<int> UpdatePassword(UpdatePasswordModel passwordModel, CancellationToken cancellationToken, int userId)
        {
            return await _repo.UpdatePassword(passwordModel, cancellationToken, userId);
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
