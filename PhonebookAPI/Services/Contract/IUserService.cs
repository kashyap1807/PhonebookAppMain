using PhonebookAPI.Dtos;

namespace PhonebookAPI.Services.Contract
{
    public interface IUserService
    {
        ServiceResponse<UserDto> GetUserByLoginId(string loginId);
        ServiceResponse<string> UpdateUser(UserDto userDto);

    }
}
