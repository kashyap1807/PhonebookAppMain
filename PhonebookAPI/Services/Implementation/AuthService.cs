using Microsoft.IdentityModel.Tokens;
using PhonebookAPI.Data.Contract;
using PhonebookAPI.Dtos;
using PhonebookAPI.Models;
using PhonebookAPI.Services.Contract;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace PhonebookAPI.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        public AuthService(IAuthRepository authRepository, IConfiguration configuration, ITokenService tokenService)
        {
            _authRepository = authRepository;
            _configuration = configuration;
            _tokenService = tokenService;
        }

        public ServiceResponse<string> RegisterUserService(RegisterDto register)
        {
            var response = new ServiceResponse<string>();
            var message = string.Empty;
            if (register != null)
            {
                message = CheckPasswordStrength(register.Password);
                if (!string.IsNullOrWhiteSpace(message))
                {
                    response.Success = false;
                    response.Message = message;
                    return response;
                }
                else if (_authRepository.UserExists(register.LoginId, register.Email))
                {
                    response.Success = false;
                    response.Message = "User Already Exists";
                    return response;
                }
                else
                {//Save User
                    User user = new User()
                    {
                        FirstName = register.FirstName,
                        LastName = register.LastName,
                        Email = register.Email,
                        LoginId = register.LoginId,
                        ContactNumber = register.ContactNumber,
                    };

                    CreatePasswordHash(register.Password, out byte[] paswordHash, out byte[] passwordSalt);
                    user.PasswordHash = paswordHash;
                    user.PasswordSalt = passwordSalt;
                    var result = _authRepository.RegisterUser(user);
                    response.Success = result;
                    response.Message = result ? string.Empty : "Something went wromg , please try after sometime";
                }
            }
            return response;
        }


        public ServiceResponse<string> LoginUserService(LoginDto login)
        {
            var response = new ServiceResponse<string>();
            //string message = string.Empty;
            if (login != null)
            {

                var user = _authRepository.ValidateUser(login.Username);
                if (user == null)
                {
                    response.Success = false;
                    response.Message = "Invalid username or password";
                    return response;
                }
                else if (!_tokenService.VerifyPasswordHash(login.Password, user.PasswordHash, user.PasswordSalt))
                {
                    response.Success = false;
                    response.Message = "Invalid username or password";
                    return response;
                }

                //string token = CreateToken(user);
                string token = _tokenService.CreateToken(user);
                response.Data = token;

                return response;
            }
            response.Message = "Something went wrong ,please try after some time";
            return response;

        }

        public ServiceResponse<string> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var response = new ServiceResponse<string>();
            if (changePasswordDto != null)
            {
                var user = _authRepository.ValidateUser(changePasswordDto.LoginId);
                if (user == null)
                {
                    response.Success = false;
                    response.Message = "Something went wrong, please try after some time.";
                    return response;
                }
                if (changePasswordDto.OldPassword == changePasswordDto.NewPassword)
                {
                    response.Success = false;
                    response.Message = "NewPassword cannot same as oldPassword";
                    return response;
                }
                if (!_tokenService.VerifyPasswordHash(changePasswordDto.OldPassword, user.PasswordHash, user.PasswordSalt))
                {
                    response.Success = false;
                    response.Message = "Old password is incorrect";
                    return response;
                }
                CheckPasswordStrength(changePasswordDto.NewPassword);
                CreatePasswordHash(changePasswordDto.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                var result = _authRepository.UpdateUser(user);
                response.Success = result;
                response.Message = result ? "Success" : "Something went wrong, please try after some time.";
            }
            else
            {
                response.Success = false;
                response.Message = "Something went wrong.";
            }
            return response;
        }

        public ServiceResponse<string> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            var response = new ServiceResponse<string>();

            if (forgotPasswordDto != null)
            {
                var user = _authRepository.ValidateUser(forgotPasswordDto.LoginId);
                if (user == null)
                {
                    response.Success = false;
                    response.Message = "Something went wrong, please try after some time.";
                    return response;
                }
                if (forgotPasswordDto.NewPassword != forgotPasswordDto.ConfirmNewPassword)
                {
                    response.Success = false;
                    response.Message = "NewPassword & ConfirmNewPassword must be same";
                    return response;
                }
                CheckPasswordStrength(forgotPasswordDto.NewPassword);
                CreatePasswordHash(forgotPasswordDto.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                var result = _authRepository.UpdateUser(user);
                response.Success = result;
                response.Message = result ? "Success" : "Something went wrong, please try after some time.";

            }
            else
            {
                response.Success = false;
                response.Message = "Something went wrong.";
            }


            return response;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }



        private string CheckPasswordStrength(string password)
        {
            StringBuilder sb = new StringBuilder();
            if (password.Length < 8)
            {
                sb.Append("Minimum Password length should be 8" + Environment.NewLine); ;
            }

            if (!Regex.IsMatch(password, "[<,>,@,!,#,$,%,^,&,*,*,(,),_,]"))
            {
                sb.Append("Password should contain special characters" + Environment.NewLine);
            }

            return sb.ToString();
        }
    }
}
