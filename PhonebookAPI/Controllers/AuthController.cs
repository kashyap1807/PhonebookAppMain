using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhonebookAPI.Dtos;
using PhonebookAPI.Services.Contract;

namespace PhonebookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public IActionResult Register(RegisterDto register)
        {
            var response = _authService.RegisterUserService(register);
            return !response.Success ? BadRequest(response) : Ok(response);
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginDto login)
        {
            var response = _authService.LoginUserService(login);
            return !response.Success ? BadRequest(response) : Ok(response);
        }

        [Authorize]
        [HttpPut("ChangePassword")]
        public IActionResult ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var response = _authService.ChangePassword(changePasswordDto);
            return !response.Success ? BadRequest(response) : Ok(response);
        }

        [HttpPut("ForgotPassword")]
        public IActionResult ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            var response = _authService.ForgotPassword(forgotPasswordDto);
            return !response.Success ? BadRequest(response) : Ok(response);
        }
    }
}
