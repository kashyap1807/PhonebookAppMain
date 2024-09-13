using Microsoft.EntityFrameworkCore;
using Moq;
using PhonebookAPI.Data.Implementation;
using PhonebookAPI.Data;
using PhonebookAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhonebookAPITests.Repositories
{
    public class StateRepositoryTests
    {
        //--------------------Get Country By Id------------------------
        [Fact]
        public void GetState_ReturnState_WhenIdFound()
        {
            //Arrange
            var id = 1;
            var state = new State { CountryId = id , StateName="State 1"};
            var states = new List<State>() { state }.AsQueryable();

            var mockDbSet = new Mock<DbSet<State>>();

            // The Expression property represents the LINQ expression tree associated with the IQueryable collection.
            mockDbSet.As<IQueryable<State>>().Setup(c => c.Expression).Returns(states.Expression);

            // The Provider property is used by LINQ query execution.
            mockDbSet.As<IQueryable<State>>().Setup(c => c.Provider).Returns(states.Provider);

            // By setting up these properties, we ensure that when methods or properties of the DbSet<Category> are invoked in the unit test,
            // they behave as expected, providing access to the LINQ query provider and expression. This allows us to mimic the behavior of a
            // real database context in our unit tests.

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.SetupGet(c => c.States).Returns(mockDbSet.Object);

            var target = new StateRepository(mockAppDbContext.Object);

            //Act
            var actual = target.GetStatesBytCountryId(id);

            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<State>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<State>>().Verify(c => c.Provider, Times.Exactly(2));
            mockAppDbContext.Verify(c => c.States, Times.Once);
        }

        [Fact]
        public void GetStateById_ReturnNull_WhenIdNotFound()
        {
            //Arrange
            var id = 1;
            var states = new List<State>() { }.AsQueryable();

            var mockDbSet = new Mock<DbSet<State>>();

            // The Expression property represents the LINQ expression tree associated with the IQueryable collection.
            mockDbSet.As<IQueryable<State>>().Setup(c => c.Expression).Returns(states.Expression);

            // The Provider property is used by LINQ query execution.
            mockDbSet.As<IQueryable<State>>().Setup(c => c.Provider).Returns(states.Provider);

            // By setting up these properties, we ensure that when methods or properties of the DbSet<Category> are invoked in the unit test,
            // they behave as expected, providing access to the LINQ query provider and expression. This allows us to mimic the behavior of a
            // real database context in our unit tests.

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.SetupGet(c => c.States).Returns(mockDbSet.Object);

            var target = new StateRepository(mockAppDbContext.Object);

            //Act
            var actual = target.GetStatesBytCountryId(id);

            //Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            mockDbSet.As<IQueryable<State>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<State>>().Verify(c => c.Provider, Times.Exactly(2));
            mockAppDbContext.Verify(c => c.States, Times.Once);
        }
    }
}
