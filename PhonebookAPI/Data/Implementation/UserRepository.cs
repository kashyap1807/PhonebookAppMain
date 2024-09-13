using PhonebookAPI.Data.Contract;
using PhonebookAPI.Models;

namespace PhonebookAPI.Data.Implementation
{
    public class UserRepository : IUserRepository
    {
        private IAppDbContext _appDbContext;

        public UserRepository(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public User? GetUserByUserId(int userId)
        {
            var users = _appDbContext.Users.FirstOrDefault(u => u.UserId == userId);
            return users;
        }

        public User? GetUserByLoginId(string loginId)
        {
            var users = _appDbContext.Users.FirstOrDefault(u=>u.LoginId == loginId || u.Email == loginId);
            return users;
        }

        public bool UpdateUSer(User user)
        {
            var result = false;
            if (user != null)
            {
                _appDbContext.Users.Update(user);
                _appDbContext.SaveChanges();
                result = true;  
            }

            return result;
        }

        public bool UserExists(int userId,string loginId, string contactNumber)
        {
            var user = _appDbContext.Users.FirstOrDefault(c=>c.UserId !=userId && (c.LoginId == loginId || c.ContactNumber == contactNumber));
            if (user != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
