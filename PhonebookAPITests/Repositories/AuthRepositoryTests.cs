using Microsoft.EntityFrameworkCore;
using Moq;
using PhonebookAPI.Data;
using PhonebookAPI.Data.Implementation;
using PhonebookAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhonebookAPITests.Repositories
{
    public class AuthRepositoryTests
    {

        [Fact]
        public void RegisterUser_ReturnsTrue()
        {
            // Arrange
            var user = new User()
            {
                UserId = 1,
                FirstName = "FirstName",
                LastName = "LastName",
                LoginId = "LoginId",
                Email = "test@example.com"
            };

            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockAppDbContext.SetupGet(o => o.Users).Returns(mockDbSet.Object);
            mockAppDbContext.Setup(o => o.SaveChanges()).Returns(1);

            var target = new AuthRepository(mockAppDbContext.Object);

            // Act
            var actual = target.RegisterUser(user);

            // Assert
            Assert.True(actual);
            mockDbSet.Verify(o => o.Add(user), Times.Once());
            mockAppDbContext.Verify(o => o.SaveChanges(), Times.Once);
        }

        [Fact]
        public void RegisterUser_ReturnsFalse()
        {
            // Arrange
            User user = null;

            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            var target = new AuthRepository(mockAppDbContext.Object);

            // Act
            var actual = target.RegisterUser(user);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void ValidateUser_ReturnsUser_WhenLoginIdIsGiven()
        {
            // Arrange
            var loginId = "loginId";
            var user = new User()
            {
                LoginId = loginId,
            };
            var users = new List<User> { user }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable<User>>().Setup(o => o.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(o => o.Expression).Returns(users.Expression);

            mockAppDbContext.Setup(o => o.Users).Returns(mockDbSet.Object);

            var target = new AuthRepository(mockAppDbContext.Object);

            // Act
            var actual = target.ValidateUser(loginId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(user, actual);
            mockDbSet.As<IQueryable<User>>().Verify(o => o.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(o => o.Expression, Times.Once);
            mockAppDbContext.Verify(o => o.Users, Times.Once);
        }

        [Fact]
        public void ValidateUser_ReturnsUser_WhenEmailIsGiven()
        {
            // Arrange
            var email = "test@example.com";
            var user = new User()
            {
                LoginId = "login",
                Email = email,
            };
            var users = new List<User> { user }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable<User>>().Setup(o => o.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(o => o.Expression).Returns(users.Expression);

            mockAppDbContext.Setup(o => o.Users).Returns(mockDbSet.Object);

            var target = new AuthRepository(mockAppDbContext.Object);

            // Act
            var actual = target.ValidateUser(email);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(user, actual);
            mockDbSet.As<IQueryable<User>>().Verify(o => o.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(o => o.Expression, Times.Once);
            mockAppDbContext.Verify(o => o.Users, Times.Once);
        }

        [Fact]
        public void ValidateUser_ReturnsNull()
        {
            // Arrange
            var loginId = "loginId";
            var user = new User()
            {
                LoginId = "temp",
            };
            var users = new List<User> { user }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable<User>>().Setup(o => o.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(o => o.Expression).Returns(users.Expression);

            mockAppDbContext.Setup(o => o.Users).Returns(mockDbSet.Object);

            var target = new AuthRepository(mockAppDbContext.Object);

            // Act
            var actual = target.ValidateUser(loginId);

            // Assert
            Assert.Null(actual);
            mockDbSet.As<IQueryable<User>>().Verify(o => o.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(o => o.Expression, Times.Once);
            mockAppDbContext.Verify(o => o.Users, Times.Once);
        }

        [Fact]
        public void UserExists_ReturnsTrue_WhenLoginIdIsGiven()
        {
            // Arrange
            var loginId = "loginId";
            var email = "test@example.com";

            var user = new User()
            {
                LoginId = loginId,
                Email = "email@example.com",
            };
            var users = new List<User>()
            {
                user,
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable<User>>().Setup(o => o.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(o => o.Expression).Returns(users.Expression);

            mockAppDbContext.Setup(o => o.Users).Returns(mockDbSet.Object);

            var target = new AuthRepository(mockAppDbContext.Object);

            // Act
            var actual = target.UserExists(loginId, email);

            // Assert
            Assert.True(actual);
            mockDbSet.As<IQueryable<User>>().Verify(o => o.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(o => o.Expression, Times.Once);
            mockAppDbContext.Verify(o => o.Users, Times.Once);
        }

        [Fact]
        public void UserExists_ReturnsTrue_WhenEmailIsGiven()
        {
            // Arrange
            var loginId = "loginId";
            var email = "test@example.com";

            var user = new User()
            {
                LoginId = "login",
                Email = email,
            };
            var users = new List<User>()
            {
                user,
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable<User>>().Setup(o => o.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(o => o.Expression).Returns(users.Expression);

            mockAppDbContext.Setup(o => o.Users).Returns(mockDbSet.Object);

            var target = new AuthRepository(mockAppDbContext.Object);

            // Act
            var actual = target.UserExists(loginId, email);

            // Assert
            Assert.True(actual);
            mockDbSet.As<IQueryable<User>>().Verify(o => o.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(o => o.Expression, Times.Once);
            mockAppDbContext.Verify(o => o.Users, Times.Once);
        }

        [Fact]
        public void UserExists_ReturnsFalse()
        {
            // Arrange
            var loginId = "loginId";
            var email = "test@example.com";

            var user = new User()
            {
                LoginId = "temp",
                Email = "email@example.com",
            };
            var users = new List<User>()
            {
                user,
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable<User>>().Setup(o => o.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(o => o.Expression).Returns(users.Expression);

            mockAppDbContext.Setup(o => o.Users).Returns(mockDbSet.Object);

            var target = new AuthRepository(mockAppDbContext.Object);

            // Act
            var actual = target.UserExists(loginId, email);

            // Assert
            Assert.False(actual);
            mockDbSet.As<IQueryable<User>>().Verify(o => o.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(o => o.Expression, Times.Once);
            mockAppDbContext.Verify(o => o.Users, Times.Once);
        }

        [Fact]
        public void UpdateUser_ReturnsTrue()
        {
            // Arrange
            var user = new User()
            {
                UserId = 1,
                FirstName = "UpdatedFirstName",
                LastName = "UpdatedLastName",
                LoginId = "UpdatedLoginId",
                Email = "updated@example.com"
            };

            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockDbSet.Setup(m => m.Update(It.IsAny<User>())).Callback<User>(u => { });
            mockAppDbContext.SetupGet(o => o.Users).Returns(mockDbSet.Object);
            mockAppDbContext.Setup(o => o.SaveChanges()).Returns(1);

            var target = new AuthRepository(mockAppDbContext.Object);

            // Act
            var actual = target.UpdateUser(user);

            // Assert
            Assert.True(actual);
            mockDbSet.Verify(o => o.Update(user), Times.Once());
            mockAppDbContext.Verify(o => o.SaveChanges(), Times.Once);
        }


        [Fact]
        public void UpdateUser_ReturnsFalse()
        {
            // Arrange
            User user = null;

            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            mockAppDbContext.SetupGet(o => o.Users).Returns(mockDbSet.Object);
            mockAppDbContext.Setup(o => o.SaveChanges()).Returns(1);

            var target = new AuthRepository(mockAppDbContext.Object);

            // Act
            var actual = target.UpdateUser(user);

            // Assert
            Assert.False(actual);
            mockDbSet.Verify(o => o.Update(It.IsAny<User>()), Times.Never());
            mockAppDbContext.Verify(o => o.SaveChanges(), Times.Never());
        }


    }

}
