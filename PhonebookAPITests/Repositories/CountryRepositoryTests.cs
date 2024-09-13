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
    public class CountryRepositoryTests
    {
        //----------Get All Countries-----
        [Fact]
        public void GetAllCountry_WhenProductExists()
        {
            // Arrange
            var countries = new List<Country>()
            {
                new Country {CountryId = 1,CountryName = "Country 1"},
                new Country {CountryId = 2,CountryName = "Country 2"}
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Country>>();
            mockDbSet.As<IQueryable<Country>>().Setup(c => c.GetEnumerator()).Returns(countries.GetEnumerator());

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Countries).Returns(mockDbSet.Object);
            var target = new CountryRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetAllCountry();

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(countries.Count(), actual.Count());
            mockAppDbContext.Verify(c => c.Countries, Times.Once);
            mockDbSet.As<IQueryable<Country>>().Verify(c => c.GetEnumerator(), Times.Once);
        }

        [Fact]
        public void GetAllCountry_WhenCountryNotExists()
        {
            var countries = new List<Country>()
            {

            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Country>>();
            mockDbSet.As<IQueryable<Country>>().Setup(c => c.GetEnumerator()).Returns(countries.GetEnumerator());

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Countries).Returns(mockDbSet.Object);
            var target = new CountryRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetAllCountry();

            // Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            Assert.Equal(countries.Count(), actual.Count());
            mockAppDbContext.Verify(c => c.Countries, Times.Once);
            mockDbSet.As<IQueryable<Country>>().Verify(c => c.GetEnumerator(), Times.Once);
        }

        //--------------------Get Country By Id------------------------
        [Fact]
        public void GetCountryById_ReturnCountry_WhenIdFound()
        {
            //Arrange
            var id = 1;
            var country = new Country { CountryId = id, CountryName = "Country 1" };
            var countries = new List<Country>() { country }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Country>>();

            // The Expression property represents the LINQ expression tree associated with the IQueryable collection.
            mockDbSet.As<IQueryable<Country>>().Setup(c => c.Expression).Returns(countries.Expression);

            // The Provider property is used by LINQ query execution.
            mockDbSet.As<IQueryable<Country>>().Setup(c => c.Provider).Returns(countries.Provider);

            // By setting up these properties, we ensure that when methods or properties of the DbSet<Category> are invoked in the unit test,
            // they behave as expected, providing access to the LINQ query provider and expression. This allows us to mimic the behavior of a
            // real database context in our unit tests.

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.SetupGet(c => c.Countries).Returns(mockDbSet.Object);

            var target = new CountryRepository(mockAppDbContext.Object);

            //Act
            var actual = target.GetCountryById(id);

            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<Country>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Country>>().Verify(c => c.Provider, Times.Once);
            mockAppDbContext.Verify(c => c.Countries, Times.Once);
        }

        [Fact]
        public void GetCOuntryById_ReturnNull_WhenIdNotFound()
        {
            //Arrange
            var id = 0;
            var countries = new List<Country>() { }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Country>>();

            // The Expression property represents the LINQ expression tree associated with the IQueryable collection.
            mockDbSet.As<IQueryable<Country>>().Setup(c => c.Expression).Returns(countries.Expression);

            // The Provider property is used by LINQ query execution.
            mockDbSet.As<IQueryable<Country>>().Setup(c => c.Provider).Returns(countries.Provider);

            // By setting up these properties, we ensure that when methods or properties of the DbSet<Category> are invoked in the unit test,
            // they behave as expected, providing access to the LINQ query provider and expression. This allows us to mimic the behavior of a
            // real database context in our unit tests.

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.SetupGet(c => c.Countries).Returns(mockDbSet.Object);

            var target = new CountryRepository(mockAppDbContext.Object);

            //Act
            var actual = target.GetCountryById(id);

            //Assert
            Assert.Null(actual);
            mockDbSet.As<IQueryable<Country>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Country>>().Verify(c => c.Provider, Times.Once);
            mockAppDbContext.Verify(c => c.Countries, Times.Once);
        }
    }
}
