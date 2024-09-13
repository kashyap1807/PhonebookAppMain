using PhonebookApp.ViewModel;

namespace PhonebookApp.Services.Contract
{
    public interface IAuthService
    {
        string RegisterUserService(RegisterViewModel register);

        string LoginUserService(LoginViewModel login);
    }
}
