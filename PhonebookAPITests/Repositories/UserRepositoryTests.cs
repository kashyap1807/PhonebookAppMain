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
    public class UserRepositoryTests
    {
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

            var target = new UserRepository(mockAppDbContext.Object);

            // Act
            var actual = target.UpdateUSer(user);

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

            var target = new UserRepository(mockAppDbContext.Object);

            // Act
            var actual = target.UpdateUSer(user);

            // Assert
            Assert.False(actual);
            mockDbSet.Verify(o => o.Update(It.IsAny<User>()), Times.Never());
            mockAppDbContext.Verify(o => o.SaveChanges(), Times.Never());
        }

        [Fact]
        public void GetUserByUserId_UserExists_ReturnsUser()
        {
            // Arrange
            var userId = 1;
            var user = new User()
            {
                UserId = userId,
                FirstName = "FirstName",
                LastName = "LastName",
                LoginId = "LoginId",
                Email = "test@example.com"
            };

            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            var data = new List<User> { user }.AsQueryable();

            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            mockAppDbContext.SetupGet(o => o.Users).Returns(mockDbSet.Object);

            var target = new UserRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetUserByUserId(userId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(userId, actual.UserId);
        }

        [Fact]
        public void GetUserByUserId_UserDoesNotExist_ReturnsNull()
        {
            // Arrange
            var userId = 1;

            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            var data = new List<User>().AsQueryable(); // Empty list

            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            mockAppDbContext.SetupGet(o => o.Users).Returns(mockDbSet.Object);

            var target = new UserRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetUserByUserId(userId);

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public void GetUserByUserId_InvalidUserId_ReturnsNull()
        {
            // Arrange
            var userId = -1;

            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            var data = new List<User>().AsQueryable(); // Empty list

            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            mockAppDbContext.SetupGet(o => o.Users).Returns(mockDbSet.Object);

            var target = new UserRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetUserByUserId(userId);

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public void GetUserByLoginId_UserExistsByLoginId_ReturnsUser()
        {
            // Arrange
            var loginId = "LoginId";
            var user = new User()
            {
                UserId = 1,
                FirstName = "FirstName",
                LastName = "LastName",
                LoginId = loginId,
                Email = "test@example.com"
            };

            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            var data = new List<User> { user }.AsQueryable();

            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            mockAppDbContext.SetupGet(o => o.Users).Returns(mockDbSet.Object);

            var target = new UserRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetUserByLoginId(loginId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(loginId, actual.LoginId);
        }

        [Fact]
        public void GetUserByLoginId_UserExistsByEmail_ReturnsUser()
        {
            // Arrange
            var loginId = "test@example.com";
            var user = new User()
            {
                UserId = 1,
                FirstName = "FirstName",
                LastName = "LastName",
                LoginId = "LoginId",
                Email = loginId
            };

            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            var data = new List<User> { user }.AsQueryable();

            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            mockAppDbContext.SetupGet(o => o.Users).Returns(mockDbSet.Object);

            var target = new UserRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetUserByLoginId(loginId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(loginId, actual.Email);
        }

        [Fact]
        public void GetUserByLoginId_UserDoesNotExist_ReturnsNull()
        {
            // Arrange
            var loginId = "NonExistentLoginId";

            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            var data = new List<User>().AsQueryable(); // Empty list

            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            mockAppDbContext.SetupGet(o => o.Users).Returns(mockDbSet.Object);

            var target = new UserRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetUserByLoginId(loginId);

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public void GetUserByLoginId_EmptyOrNullLoginId_ReturnsNull()
        {
            // Arrange
            string loginId = null;

            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            var data = new List<User>().AsQueryable(); // Empty list

            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            mockAppDbContext.SetupGet(o => o.Users).Returns(mockDbSet.Object);

            var target = new UserRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetUserByLoginId(loginId);

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public void UserExists_UserExistsWithSameLoginId_ReturnsTrue()
        {
            // Arrange
            var userId = 1;
            var loginId = "LoginId";
            var contactNumber = "1234567890";

            var existingUser = new User()
            {
                UserId = 2, // Different UserId
                LoginId = loginId,
                ContactNumber = "0987654321"
            };

            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            var data = new List<User> { existingUser }.AsQueryable();

            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            mockAppDbContext.SetupGet(o => o.Users).Returns(mockDbSet.Object);

            var target = new UserRepository(mockAppDbContext.Object);

            // Act
            var actual = target.UserExists(userId, loginId, contactNumber);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void UserExists_UserExistsWithSameContactNumber_ReturnsTrue()
        {
            // Arrange
            var userId = 1;
            var loginId = "AnotherLoginId";
            var contactNumber = "1234567890";

            var existingUser = new User()
            {
                UserId = 2, // Different UserId
                LoginId = "DifferentLoginId",
                ContactNumber = contactNumber
            };

            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            var data = new List<User> { existingUser }.AsQueryable();

            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            mockAppDbContext.SetupGet(o => o.Users).Returns(mockDbSet.Object);

            var target = new UserRepository(mockAppDbContext.Object);

            // Act
            var actual = target.UserExists(userId, loginId, contactNumber);

            // Assert
            Assert.True(actual);
        }
        [Fact]
        public void UserExists_UserDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var userId = 1;
            var loginId = "LoginId";
            var contactNumber = "1234567890";

            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            var data = new List<User>().AsQueryable(); // Empty list

            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            mockAppDbContext.SetupGet(o => o.Users).Returns(mockDbSet.Object);

            var target = new UserRepository(mockAppDbContext.Object);

            // Act
            var actual = target.UserExists(userId, loginId, contactNumber);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void UserExists_UserExistsWithSameUserId_ReturnsFalse()
        {
            // Arrange
            var userId = 1;
            var loginId = "LoginId";
            var contactNumber = "1234567890";

            var existingUser = new User()
            {
                UserId = userId, // Same UserId
                LoginId = loginId,
                ContactNumber = contactNumber
            };

            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            var data = new List<User> { existingUser }.AsQueryable();

            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            mockAppDbContext.SetupGet(o => o.Users).Returns(mockDbSet.Object);

            var target = new UserRepository(mockAppDbContext.Object);

            // Act
            var actual = target.UserExists(userId, loginId, contactNumber);

            // Assert
            Assert.False(actual);
        }

    }
}
