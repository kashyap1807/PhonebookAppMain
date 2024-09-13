using PhonebookAPI.Data.Contract;
using PhonebookAPI.Dtos;
using PhonebookAPI.Services.Contract;

namespace PhonebookAPI.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public UserService(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public ServiceResponse<UserDto> GetUserByLoginId(string loginId)
        {
            var response = new ServiceResponse<UserDto>();
            try
            {
                var users = _userRepository.GetUserByLoginId(loginId);
                if (users != null)
                {
                    var userDto = new UserDto
                    {
                        userId = users.UserId,
                        FirstName = users.FirstName,
                        LastName = users.LastName,
                        LoginId = users.LoginId,
                        Email = users.Email,
                        ContactNumber = users.ContactNumber,


                    };
                    response.Data = userDto;
                    response.Success = true;
                    response.Message = "Record Found";
                }
                else
                {
                    response.Success = false;
                    response.Message = "No Record Found";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            
            return response;
        }

        public ServiceResponse<string> UpdateUser(UserDto userDto)
        {
            var response = new ServiceResponse<string>();
            try
            {
                if (userDto == null)
                {
                    response.Success = false;
                    response.Message = "Something went wrong. Please try after sometime.";
                    return response;
                }
                if (_userRepository.UserExists(userDto.userId, userDto.LoginId, userDto.ContactNumber))
                {
                    response.Success = false;
                    response.Message = "Contact already exists.";
                    return response;
                }

                var modifyUser = _userRepository.GetUserByUserId(userDto.userId);
                if (modifyUser != null)
                {
                    modifyUser.FirstName = userDto.FirstName;
                    modifyUser.LastName = userDto.LastName;
                    modifyUser.LoginId = userDto.LoginId;
                    modifyUser.Email = userDto.Email;
                    modifyUser.ContactNumber = userDto.ContactNumber;

                    var result = _userRepository.UpdateUSer(modifyUser);
                    if (result == true)
                    {
                        string token = _tokenService.CreateToken(modifyUser);
                        response.Data = token;
                        response.Success = true;
                        response.Message = result ? "User updated successfully." : "Something went wrong. Please try after sometime.";

                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Something went wrong. Please try after sometime.";

                    }

                }
                else
                {
                    response.Success = false;
                    response.Message = "Something went wrong. Please try after sometime.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            
            return response;

        }
    }
}
