using UserInformationApp.Domain;

namespace UserInformationApp.Contracts
{
    public interface IUser
    {
        Task<IEnumerable<User>> GetAllUsersAsync(int page);
        Task<User?> GetUserByIdAsync(int userId);
    }
}
