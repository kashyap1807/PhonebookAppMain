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
    public class UserServiceTests
    {
        [Fact]
        public void GetUserByLoginId_ValidUser_ReturnsUserDto()
        {
            // Arrange
            string loginId = "testUser";
            var user = new User
            {
                UserId = 1,
                FirstName = "John",
                LastName = "Doe",
                LoginId = loginId,
                Email = "john.doe@example.com",
                ContactNumber = "1234567890"
            };

            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repo => repo.GetUserByLoginId(loginId)).Returns(user);

            var authService = new UserService(mockUserRepository.Object,null);

            // Act
            var actual = authService.GetUserByLoginId(loginId);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(user.UserId, actual.Data.userId);
            Assert.Equal(user.FirstName, actual.Data.FirstName);
            Assert.Equal(user.LastName, actual.Data.LastName);
            Assert.Equal(user.LoginId, actual.Data.LoginId);
            Assert.Equal(user.Email, actual.Data.Email);
            Assert.Equal(user.ContactNumber, actual.Data.ContactNumber);
            mockUserRepository.Verify(repo => repo.GetUserByLoginId(loginId), Times.Once);
        }

        [Fact]
        public void UpdateUser_ValidUserDto_ReturnsSuccessResponse()
        {
            // Arrange
            var userDto = new UserDto
            {
                userId = 1,
                FirstName = "FirstName",
                LastName = "LastName",
                LoginId = "first",
                Email = "first.name@example.com",
                ContactNumber = "1234567890"
            };

            var modifyUser = new User
            {
                UserId = userDto.userId,
                FirstName = "OldFirstName",
                LastName = "OldLastName",
                LoginId = "old.login",
                Email = "old@example.com",
                ContactNumber = "9876543210"
            };

            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repo => repo.GetUserByUserId(userDto.userId)).Returns(modifyUser);
            mockUserRepository.Setup(repo => repo.UserExists(userDto.userId, userDto.LoginId, userDto.ContactNumber)).Returns(false);
            mockUserRepository.Setup(repo => repo.UpdateUSer(modifyUser)).Returns(true);

            var mockTokenService = new Mock<ITokenService>();
            mockTokenService.Setup(service => service.CreateToken(modifyUser)).Returns("fakeToken");

            var userService = new UserService(mockUserRepository.Object, mockTokenService.Object);

            // Act
            var actual = userService.UpdateUser(userDto);

            // Assert
            Assert.True(actual.Success);
            Assert.Equal("User updated successfully.", actual.Message);
            Assert.Equal("fakeToken", actual.Data);
            mockUserRepository.Verify(repo => repo.GetUserByUserId(userDto.userId), Times.Once);
            mockUserRepository.Verify(repo => repo.UpdateUSer(modifyUser), Times.Once);
            mockTokenService.Verify(service => service.CreateToken(modifyUser), Times.Once);
        }
        [Fact]
        public void UpdateUser_ValidUserDto_ReturnsErrorResponse()
        {
            // Arrange
            var userDto = new UserDto
            {
                userId = 1,
                FirstName = "FirstName",
                LastName = "LastName",
                LoginId = "first",
                Email = "first.name@example.com",
                ContactNumber = "1234567890"
            };

            var modifyUser = new User
            {
                UserId = userDto.userId,
                FirstName = "OldFirstName",
                LastName = "OldLastName",
                LoginId = "old.login",
                Email = "old@example.com",
                ContactNumber = "9876543210"
            };

            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repo => repo.GetUserByUserId(userDto.userId)).Returns(modifyUser);
            mockUserRepository.Setup(repo => repo.UserExists(userDto.userId, userDto.LoginId, userDto.ContactNumber)).Returns(false);
            mockUserRepository.Setup(repo => repo.UpdateUSer(modifyUser)).Returns(false);

            var mockTokenService = new Mock<ITokenService>();
            mockTokenService.Setup(service => service.CreateToken(modifyUser)).Returns("fakeToken");

            var userService = new UserService(mockUserRepository.Object, mockTokenService.Object);

            // Act
            var actual = userService.UpdateUser(userDto);

            // Assert
            Assert.False(actual.Success);
            Assert.Equal("Something went wrong. Please try after sometime.", actual.Message);
            Assert.Equal(null, actual.Data);
            mockUserRepository.Verify(repo => repo.GetUserByUserId(userDto.userId), Times.Once);
            mockUserRepository.Verify(repo => repo.UpdateUSer(modifyUser), Times.Once);
           
        }
        [Fact]
        public void UpdateUser_UserNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var userDto = new UserDto
            {
                userId = 1,
                FirstName = "FirstName",
                LastName = "LastName",
                LoginId = "first",
                Email = "first.name@example.com",
                ContactNumber = "1234567890"
            };

            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repo => repo.GetUserByUserId(userDto.userId)).Returns((User)null);

            var userService = new UserService(mockUserRepository.Object, null);

            // Act
            var actual = userService.UpdateUser(userDto);

            // Assert
            Assert.False(actual.Success);
            Assert.Equal("Something went wrong. Please try after sometime.", actual.Message);
            mockUserRepository.Verify(repo => repo.GetUserByUserId(userDto.userId), Times.Once);
            mockUserRepository.Verify(repo => repo.UpdateUSer(It.IsAny<User>()), Times.Never);
        }
        [Fact]
        public void UpdateUser_DuplicateContactNumber_ReturnsErrorResponse()
        {
            // Arrange
            var userDto = new UserDto
            {
                userId = 1,
                FirstName = "FirstName",
                LastName = "LastName",
                LoginId = "first",
                Email = "first.name@example.com",
                ContactNumber = "1234567890"
            };

            var modifyUser = new User
            {
                UserId = userDto.userId,
                FirstName = "OldFirstName",
                LastName = "OldLastName",
                LoginId = "old.login",
                Email = "old@example.com",
                ContactNumber = "1234567890" // Matching contact number
            };

            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repo => repo.GetUserByUserId(userDto.userId)).Returns(modifyUser);
            mockUserRepository.Setup(repo => repo.UserExists(userDto.userId, userDto.LoginId, userDto.ContactNumber)).Returns(true);

            var userService = new UserService(mockUserRepository.Object, null);

            // Act
            var actual = userService.UpdateUser(userDto);

            // Assert
            Assert.False(actual.Success);
            Assert.Equal("Contact already exists.", actual.Message);
            mockUserRepository.Verify(repo => repo.UpdateUSer(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public void UpdateUser_NullUserDto_ReturnsErrorResponse()
        {
            // Arrange
            UserDto userDto = null;

            var userService = new UserService(null, null);

            // Act
            var actual = userService.UpdateUser(userDto);

            // Assert
            Assert.False(actual.Success);
            Assert.Equal("Something went wrong. Please try after sometime.", actual.Message);
        }


    }
}
