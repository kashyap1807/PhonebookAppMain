using Microsoft.AspNetCore.Mvc;
using Moq;
using PhonebookAPI.Controllers;
using PhonebookAPI.Dtos;
using PhonebookAPI.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhonebookAPITests.Controllers
{
    public class UserControllerTests
    {
        [Fact]
        public void GetUserByLoginId_ReturnsOk_WhenUserFound()
        {
            // Arrange
            var loginId = "username";
            var expectedUserDto = new UserDto
            {
                userId = 1,
                FirstName = "John",
                LastName = "Doe",
                LoginId = loginId,
                Email = "john.doe@example.com",
                ContactNumber = "1234567890"
            };

            var expectedServiceResponse = new ServiceResponse<UserDto>
            {
                Success = true,
                Data = expectedUserDto
            };

            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.GetUserByLoginId(loginId))
                           .Returns(expectedServiceResponse);

            var userController = new UserController(mockUserService.Object);

            // Act
            var actual = userController.GetUserByLoginId(loginId) as ObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            mockUserService.Verify(service => service.GetUserByLoginId(loginId), Times.Once);
        }
        [Fact]
        public void GetUserByLoginId_ReturnsNotFound_WhenUserNotFound()
        {
            // Arrange
            var loginId = "nonexistentuser";

            var expectedServiceResponse = new ServiceResponse<UserDto>
            {
                Success = false,
                Message = "User not found"
            };

            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.GetUserByLoginId(loginId))
                           .Returns(expectedServiceResponse);

            var userController = new UserController(mockUserService.Object);

            // Act
            var actual = userController.GetUserByLoginId(loginId) as ObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            mockUserService.Verify(service => service.GetUserByLoginId(loginId), Times.Once);
        }
        [Fact]
        public void UpdateUser_ReturnsOk_WhenUpdateSucceeds()
        {
            // Arrange
            var userDto = new UserDto
            {
                userId = 1,
                FirstName = "John",
                LastName = "Doe",
                LoginId = "johndoe",
                Email = "john.doe@example.com",
                ContactNumber = "1234567890"
            };

            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,
                Message = "User updated successfully."
            };

            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.UpdateUser(userDto))
                           .Returns(expectedServiceResponse);

            var userController = new UserController(mockUserService.Object);

            // Act
            var result = userController.UpdateUser(userDto) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);
            mockUserService.Verify(service => service.UpdateUser(userDto), Times.Once);
        }
        [Fact]
        public void UpdateUser_ReturnsBadRequest_WhenUpdateFails()
        {
            // Arrange
            var userDto = new UserDto
            {
                userId = 1,
                FirstName = "John",
                LastName = "Doe",
                LoginId = "johndoe",
                Email = "john.doe@example.com",
                ContactNumber = "1234567890"
            };

            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = "Contact already exists."
            };

            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.UpdateUser(userDto))
                           .Returns(expectedServiceResponse);

            var userController = new UserController(mockUserService.Object);

            // Act
            var result = userController.UpdateUser(userDto) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.NotNull(result.Value);
            mockUserService.Verify(service => service.UpdateUser(userDto), Times.Once);
        }

    }
}
