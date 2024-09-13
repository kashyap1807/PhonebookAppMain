using Microsoft.IdentityModel.Tokens;
using PhonebookApp.Data.Contract;
using PhonebookApp.Models;
using PhonebookApp.Services.Contract;
using PhonebookApp.ViewModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Text;

namespace PhonebookApp.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IAuthRepository authRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }

        public string RegisterUserService(RegisterViewModel register)
        {
            var message = string.Empty;
            if (register != null)
            {
                message = CheckPasswordStrength(register.Password);
                if (!string.IsNullOrWhiteSpace(message))
                {
                    return message;
                }
                else if (_authRepository.UserExists(register.LoginId, register.Email))
                {
                    message = "User Already Exists";
                    return message;
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
                    message = result ? string.Empty : "Something went wromg , please try after sometime";
                }
            }
            return message;
        }

        public string LoginUserService(LoginViewModel login)
        {
            string message = string.Empty;
            if (login != null)
            {
                message = "Invalid username or password";
                var user = _authRepository.ValidateUser(login.Username);
                if (user == null)
                {
                    return message;
                }
                else if (!VerifyPasswordHash(login.Password, user.PasswordHash, user.PasswordSalt))
                {
                    return message;
                }

                string token = CreateToken(user);

                return token;
            }
            message = "Something went wrong ,please try after some time";
            return message;

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

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }

        public string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier , user.userId.ToString()),
                new Claim(ClaimTypes.Name , user.LoginId.ToString()),
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            SigningCredentials sc = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = sc
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
