using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhonebookAPI.Dtos;
using PhonebookAPI.Services.Contract;

namespace PhonebookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetUserByLoginId/{loginId}")]
        public IActionResult GetUserByLoginId(string loginId)
        {
            var response = _userService.GetUserByLoginId(loginId);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPut("UpdateUser")]
        public IActionResult UpdateUser(UserDto userDto)
        {
            var result = _userService.UpdateUser(userDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            else
            {
                return Ok(result);
            }
        }

    }
}
