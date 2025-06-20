using UserInformationApp.Domain;

namespace UserInformationApp.Contracts
{
    public interface IApiClient
    {
        Task<User?> GetUserByIdAsync(int userId);
        Task<IEnumerable<User>> GetAllUsersAsync(int page);
    }
}