using Microsoft.Extensions.Configuration;
using Moq;
using PhonebookAPI.Data.Contract;
using PhonebookAPI.Dtos;
using PhonebookAPI.Models;
using PhonebookAPI.Services.Contract;
using PhonebookAPI.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhonebookAPITests.Services
{
    public class AuthServiceTests
    {
        //-------------------------------Register User--------------------------
        [Fact]
        public void RegisterUserService_ReturnsSuccess_WhenValidRegistration()
        {
            // Arrange
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockTokenService = new Mock<ITokenService>();
            mockAuthRepository.Setup(repo => repo.UserExists(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            mockAuthRepository.Setup(repo => repo.RegisterUser(It.IsAny<User>())).Returns(true);


            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object, mockTokenService.Object);

            var registerDto = new RegisterDto
            {
                FirstName = "firstname",
                LastName = "lastname",
                Email = "email@example.com",
                LoginId = "loginid",
                ContactNumber = "1234567890",
                Password = "Password@123"
            };

            // Act
            var actual = target.RegisterUserService(registerDto);

            // Assert
            Assert.True(actual.Success);
            Assert.Equal(string.Empty, actual.Message);
            mockAuthRepository.Verify(c => c.UserExists(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockAuthRepository.Verify(c => c.RegisterUser(It.IsAny<User>()), Times.Once);
        }


        [Fact]
        public void RegisterUserService_ReturnsFailure_WhenRegistrationFails()
        {
            // Arrange
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockTokenService = new Mock<ITokenService>();
            mockAuthRepository.Setup(repo => repo.UserExists(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            mockAuthRepository.Setup(repo => repo.RegisterUser(It.IsAny<User>())).Returns(false);


            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object, mockTokenService.Object);

            var registerDto = new RegisterDto
            {
                FirstName = "firstname",
                LastName = "lastname",
                Email = "email@example.com",
                LoginId = "loginid",
                ContactNumber = "1234567890",
                Password = "Password@123"
            };

            // Act
            var actual = target.RegisterUserService(registerDto);

            // Assert
            Assert.False(actual.Success);
            Assert.Equal("Something went wromg , please try after sometime", actual.Message);
            mockAuthRepository.Verify(c => c.UserExists(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockAuthRepository.Verify(c => c.RegisterUser(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public void RegisterUserService_ReturnsFailure_WhenMessageIsNull()
        {
            // Arrange
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockTokenService = new Mock<ITokenService>();
            mockAuthRepository.Setup(repo => repo.UserExists(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            mockAuthRepository.Setup(repo => repo.RegisterUser(It.IsAny<User>())).Returns(false);


            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object, mockTokenService.Object);

            var registerDto = new RegisterDto
            {
                FirstName = "firstname",
                LastName = "lastname",
                Email = "email@example.com",
                LoginId = "loginid",
                ContactNumber = "1234567890",
                Password = "",
            };

            // Act
            var actual = target.RegisterUserService(registerDto);

            // Assert
            Assert.False(actual.Success);
        }

        [Fact]
        public void RegisterUserService_ReturnsEmptyResponse_WhenRegisterIsNull()
        {
            // Arrange
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockTokenService = new Mock<ITokenService>();
            RegisterDto registerDto = null;
            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object, mockTokenService.Object);

            // Act
            var actual = target.RegisterUserService(registerDto);

            // Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            //Assert.Equal(string.Empty, result.Message);
        }

        [Fact]
        public void RegisterUserService_ReturnsFailure_WhenUserAlready_Exists()
        {

            // Arrange
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockTokenService = new Mock<ITokenService>();
            var registerDto = new RegisterDto
            {
                FirstName = "firstname",
                LastName = "lastname",
                Email = "email@example.com",
                LoginId = "loginid",
                ContactNumber = "1234567890",
                Password = "Password@123"
            };
            mockAuthRepository.Setup(repo => repo.UserExists(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object, mockTokenService.Object);

            // Act
            var actual = target.RegisterUserService(registerDto);

            // Assert
            Assert.False(actual.Success);
            Assert.Equal("User Already Exists", actual.Message);
            mockAuthRepository.Verify(c => c.UserExists(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }


        //----------------------------------Login User-------------------------
        [Fact]
        public void LoginUserService_ReturnsErrorMessage_WhenLoginIsNull()
        {
            // Arrange
            LoginDto loginDto = null;
            var errorMessage = "Something went wrong ,please try after some time";

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockTokenService = new Mock<ITokenService>();

            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object, mockTokenService.Object);

            // Act
            var actual = target.LoginUserService(loginDto);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(errorMessage, actual.Message);
        }

        [Fact]
        public void LoginUserService_ReturnsError_WhenUsernameIsNotValid()
        {
            // Arrange
            LoginDto loginDto = new LoginDto()
            {
                Username = "username",
                Password = "Password@123",
            };
            var errorMessage = "Invalid username or password";

            var response = new ServiceResponse<string>
            {
                Success = false,
                Message = errorMessage,
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockTokenService = new Mock<ITokenService>();

            mockAuthRepository.Setup(o => o.ValidateUser(loginDto.Username)).Returns<User>(null);

            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object, mockTokenService.Object);

            // Act
            var actual = target.LoginUserService(loginDto);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Success, actual.Success);
            Assert.Equal(response.Message, actual.Message);
            mockAuthRepository.Verify(o => o.ValidateUser(loginDto.Username), Times.Once);
        }

        [Fact]
        public void LoginUserService_ReturnsError_WhenPasswordIsInvalid()
        {
            // Arrange
            LoginDto loginViewModel = new LoginDto()
            {
                Username = "username",
                Password = "Password@123",
            };
            var errorMessage = "Invalid username or password";

            var response = new ServiceResponse<string>
            {
                Success = false,
                Message = errorMessage,
            };

            User user = new User();

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockTokenService = new Mock<ITokenService>();

            mockAuthRepository.Setup(o => o.ValidateUser(loginViewModel.Username)).Returns(user);
            mockTokenService.Setup(o => o.VerifyPasswordHash(loginViewModel.Password, It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(false);
            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object, mockTokenService.Object);

            // Act
            var actual = target.LoginUserService(loginViewModel);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Success, actual.Success);
            Assert.Equal(response.Message, actual.Message);
            mockAuthRepository.Verify(o => o.ValidateUser(loginViewModel.Username), Times.Once);
            mockTokenService.Verify(o => o.VerifyPasswordHash(loginViewModel.Password, It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Once);
        }

        [Fact]
        public void LoginUserService_ReturnsToken_WhenLoginSuccessful()
        {
            // Arrange
            LoginDto loginViewModel = new LoginDto()
            {
                Username = "username",
                Password = "Password@123",
            };
            var token = "fakeToken";

            var response = new ServiceResponse<string>
            {
                Data = token,
                Success = false,
            };
            User user = new User();

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockTokenService = new Mock<ITokenService>();

            mockAuthRepository.Setup(o => o.ValidateUser(loginViewModel.Username)).Returns(user);
            mockTokenService.Setup(o => o.VerifyPasswordHash(loginViewModel.Password, It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(true);
            mockTokenService.Setup(o => o.CreateToken(user)).Returns(token);

            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object, mockTokenService.Object);

            // Act
            var actual = target.LoginUserService(loginViewModel);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(token, actual.Data);
            mockTokenService.Verify(o => o.VerifyPasswordHash(loginViewModel.Password, It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Once);
            mockTokenService.Verify(o => o.CreateToken(user), Times.Once);
        }

        [Fact]
        public void ChangePassword_NullChangePasswordDto_ReturnsErrorResponse()
        {
            // Arrange
            ChangePasswordDto changePasswordDto = null;
            var expectedResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = "Something went wrong."
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();
            var mockConfiguration = new Mock<IConfiguration>();

            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object, mockTokenService.Object);

            // Act
            var actual = target.ChangePassword(changePasswordDto);

            // Assert
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
        }

        [Fact]
        public void ChangePassword_UserNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var changePasswordDto = new ChangePasswordDto
            {
                LoginId = "loginId",
                OldPassword = "OldPassword",
                NewPassword = "NewPassword"
            };
            var expectedResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = "Something went wrong, please try after some time."
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();
            var mockConfiguration = new Mock<IConfiguration>();

            mockAuthRepository.Setup(o => o.ValidateUser(changePasswordDto.LoginId)).Returns((User)null);

            var target = new AuthService(mockAuthRepository.Object,mockConfiguration.Object, mockTokenService.Object);

            // Act
            var actual = target.ChangePassword(changePasswordDto);

            // Assert
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockAuthRepository.Verify(o => o.ValidateUser(changePasswordDto.LoginId), Times.Once);
        }
        [Fact]
        public void ChangePassword_OldPasswordSameAsNewPassword_ReturnsErrorResponse()
        {
            // Arrange
            var changePasswordDto = new ChangePasswordDto
            {
                LoginId = "loginId",
                OldPassword = "Password@123",
                NewPassword = "Password@123"
            };
            var user = new User
            {
                PasswordHash = new byte[] { },
                PasswordSalt = new byte[] { }
            };
            var expectedResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = "NewPassword cannot same as oldPassword"
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockAuthRepository.Setup(o => o.ValidateUser(changePasswordDto.LoginId)).Returns(user);
            var target = new AuthService(mockAuthRepository.Object,mockConfiguration.Object, mockTokenService.Object);

            // Act
            var actual = target.ChangePassword(changePasswordDto);

            // Assert
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
        }
        [Fact]
        public void ChangePassword_OldPasswordIncorrect_ReturnsErrorResponse()
        {
            // Arrange
            var changePasswordDto = new ChangePasswordDto
            {
                LoginId = "loginId",
                OldPassword = "OldPassword",
                NewPassword = "NewPassword"
            };
            var user = new User
            {
                PasswordHash = new byte[] { },
                PasswordSalt = new byte[] { }
            };
            var expectedResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = "Old password is incorrect"
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();
            var mockConfiguration = new Mock<IConfiguration>();

            mockAuthRepository.Setup(o => o.ValidateUser(changePasswordDto.LoginId)).Returns(user);
            mockTokenService.Setup(o => o.VerifyPasswordHash(changePasswordDto.OldPassword, user.PasswordHash, user.PasswordSalt)).Returns(false);

            var target = new AuthService(mockAuthRepository.Object,mockConfiguration.Object, mockTokenService.Object);

            // Act
            var actual = target.ChangePassword(changePasswordDto);

            // Assert
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockAuthRepository.Verify(o => o.ValidateUser(changePasswordDto.LoginId), Times.Once);
            mockTokenService.Verify(o => o.VerifyPasswordHash(changePasswordDto.OldPassword, user.PasswordHash, user.PasswordSalt), Times.Once);
        }
        [Fact]
        public void ChangePassword_Successful_ReturnsSuccessResponse()
        {
            // Arrange
            var changePasswordDto = new ChangePasswordDto
            {
                LoginId = "loginId",
                OldPassword = "OldPassword",
                NewPassword = "NewPassword"
            };
            var user = new User
            {
                PasswordHash = new byte[] { },
                PasswordSalt = new byte[] { }
            };
            var expectedResponse = new ServiceResponse<string>
            {
                Success = true,
                Message = "Success"
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();
            var mockConfiguration = new Mock<IConfiguration>();

            mockAuthRepository.Setup(o => o.ValidateUser(changePasswordDto.LoginId)).Returns(user);
            mockTokenService.Setup(o => o.VerifyPasswordHash(changePasswordDto.OldPassword, user.PasswordHash, user.PasswordSalt)).Returns(true);
            mockAuthRepository.Setup(o => o.UpdateUser(user)).Returns(true);

            var target = new AuthService(mockAuthRepository.Object,mockConfiguration.Object, mockTokenService.Object);

            // Act
            var actual = target.ChangePassword(changePasswordDto);

            // Assert
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockAuthRepository.Verify(o => o.ValidateUser(changePasswordDto.LoginId), Times.Once);
            mockAuthRepository.Verify(o => o.UpdateUser(user), Times.Once);
        }
        [Fact]
        public void ForgotPassword_NullForgotPasswordDto_ReturnsErrorResponse()
        {
            // Arrange
            ForgotPasswordDto forgotPasswordDto = null;
            var expectedResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = "Something went wrong."
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var authService = new AuthService(mockAuthRepository.Object, null, null);

            // Act
            var actual = authService.ForgotPassword(forgotPasswordDto);

            // Assert
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
        }
        [Fact]
        public void ForgotPassword_UserNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var forgotPasswordDto = new ForgotPasswordDto
            {
                LoginId = "loginId",
                NewPassword = "NewPassword",
                ConfirmNewPassword = "NewPassword"
            };
            var expectedResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = "Something went wrong, please try after some time."
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var authService = new AuthService(mockAuthRepository.Object, null, null);

            mockAuthRepository.Setup(o => o.ValidateUser(forgotPasswordDto.LoginId)).Returns((User)null);

            // Act
            var actual = authService.ForgotPassword(forgotPasswordDto);

            // Assert
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockAuthRepository.Verify(o => o.ValidateUser(forgotPasswordDto.LoginId), Times.Once);
        }
        [Fact]
        public void ForgotPassword_NewPasswordMismatch_ReturnsErrorResponse()
        {
            // Arrange
            var forgotPasswordDto = new ForgotPasswordDto
            {
                LoginId = "loginId",
                NewPassword = "NewPassword",
                ConfirmNewPassword = "MismatchedPassword"
            };
            var expectedResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = "NewPassword & ConfirmNewPassword must be same"
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var authService = new AuthService(mockAuthRepository.Object, null, null);

            var user = new User();
            mockAuthRepository.Setup(o => o.ValidateUser(forgotPasswordDto.LoginId)).Returns(user);

            // Act
            var actual = authService.ForgotPassword(forgotPasswordDto);

            // Assert
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockAuthRepository.Verify(o => o.ValidateUser(forgotPasswordDto.LoginId), Times.Once);
        }
        [Fact]
        public void ForgotPassword_Successful_ReturnsSuccessResponse()
        {
            // Arrange
            var forgotPasswordDto = new ForgotPasswordDto
            {
                LoginId = "loginId",
                NewPassword = "NewPassword",
                ConfirmNewPassword = "NewPassword"
            };
            var expectedResponse = new ServiceResponse<string>
            {
                Success = true,
                Message = "Success"
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var authService = new AuthService(mockAuthRepository.Object, null, null);

            var user = new User();
            mockAuthRepository.Setup(o => o.ValidateUser(forgotPasswordDto.LoginId)).Returns(user);
            mockAuthRepository.Setup(o => o.UpdateUser(user)).Returns(true);

            // Act
            var actual = authService.ForgotPassword(forgotPasswordDto);

            // Assert
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockAuthRepository.Verify(o => o.ValidateUser(forgotPasswordDto.LoginId), Times.Once);
            mockAuthRepository.Verify(o => o.UpdateUser(user), Times.Once);
        }
       

    }
}
