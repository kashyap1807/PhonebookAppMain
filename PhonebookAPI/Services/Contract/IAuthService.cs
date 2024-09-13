using PhonebookAPI.Dtos;

namespace PhonebookAPI.Services.Contract
{

    public interface IAuthService
    {
        ServiceResponse<string> RegisterUserService(RegisterDto register);

        ServiceResponse<string> LoginUserService(LoginDto login);
        ServiceResponse<string> ChangePassword(ChangePasswordDto changePasswordDto);

        ServiceResponse<string> ForgotPassword(ForgotPasswordDto forgotPasswordDto);
    }
}
