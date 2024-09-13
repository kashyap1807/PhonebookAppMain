using PhonebookAPI.Models;

namespace PhonebookAPI.Data.Contract
{
    public interface IAuthRepository
    {
        bool RegisterUser(User user);
        bool UpdateUser(User user);
        User ValidateUser(string username);

        bool UserExists(string loginId, string email);
    }
}
