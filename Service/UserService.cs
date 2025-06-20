using UserInformationApp.Contracts;
using UserInformationApp.Domain;
using UserInformationApp.Infrastructure;

namespace UserInformationApp.Service
{
    public class UserService : IUser
    {
        private readonly IApiClient _client;
        public UserService(IApiClient client) => _client = client;

        public async Task<IEnumerable<User>> GetAllUsersAsync(int page) =>
            await _client.GetAllUsersAsync(page);

        public async Task<User?> GetUserByIdAsync(int userId) =>
            await _client.GetUserByIdAsync(userId);
    }
}
