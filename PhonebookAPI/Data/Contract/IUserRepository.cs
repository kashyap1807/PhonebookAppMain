using PhonebookAPI.Models;

namespace PhonebookAPI.Data.Contract
{
    public interface IUserRepository
    {
        User? GetUserByUserId(int userId);
        User? GetUserByLoginId(string loginId);

        bool UpdateUSer(User user);

        bool UserExists(int userId, string loginId, string contactNumber);
    }
}
