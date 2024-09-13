using Moq;
using PhonebookAPI.Data.Contract;
using PhonebookAPI.Dtos;
using PhonebookAPI.Models;
using PhonebookAPI.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhonebookAPITests.Services
{
    public class CountryServiceTests
    {

        //---------------GetAll Countries--------------
        [Fact]
        public void GetAllCountry_ReturnSucess_WhenCountryFound()
        {
            //Arrange
            IEnumerable<Country> country = new List<Country>()
            {
                new Country() { CountryId=1,CountryName="test1" },
                new Country() { CountryId=2,CountryName="test2" },
            };

            var response = new ServiceResponse<IEnumerable< CountryDto>>()
            {
                
                Success=true,
                Message= "Success",
            };

            var mockCountryRepos = new Mock<ICountryRepository>();
            mockCountryRepos.Setup(c => c.GetAllCountry()).Returns(country);

            var target = new CountryService(mockCountryRepos.Object);
            
            //Act
            var actual = target.GetAllCountry();

            //Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            Assert.Equal(response.Message, actual.Message);
            mockCountryRepos.Verify(c => c.GetAllCountry(),Times.Once);

        }
        [Fact]
        public void GetAllCountires_ReturnsErrorMessage_WhenNoCountriesExists()
        {
            // Arranage
            var countries = new List<Country>();
            var response = new ServiceResponse<IEnumerable<Country>>()
            {
                Data = countries,
                Success = false,
                Message = "No record found",
            };
            var mockCountryRepository = new Mock<ICountryRepository>();
            var target = new CountryService(mockCountryRepository.Object);

            mockCountryRepository.Setup(x => x.GetAllCountry()).Returns(countries);

            // Act
            var actual = target.GetAllCountry();

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Message, actual.Message);
            mockCountryRepository.Verify(x => x.GetAllCountry(), Times.Once);
        }

        //-----------------Get Country By Id---------------
        [Fact]
        public void GetCountryById_ReturnsSuccess_WhenCountryExists()
        {
            // Arrange
            var country = new Country { CountryId = 1, CountryName = "Country1" };
            var response = new ServiceResponse<Country>()
            {
                Data = country,
                Success = true,
                Message = null,
            };
            var mockCountryRepository = new Mock<ICountryRepository>();
            var target = new CountryService(mockCountryRepository.Object);

            mockCountryRepository.Setup(c => c.GetCountryById(country.CountryId)).Returns(country);

            // Act
            var actual = target.GetCoutryById(country.CountryId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Message, actual.Message);
            mockCountryRepository.Verify(c => c.GetCountryById(country.CountryId), Times.Once);
        }
        [Fact]
        public void GetCountryById_ReturnsFailure_WhenCountryNotExists()
        {
            // Arrange
            var countryId = 1;
            Country country = null;

            var response = new ServiceResponse<Country>()
            {
                Data = country,
                Success = false,
                Message = "No record found ! ",
            };
            var mockCountryRepository = new Mock<ICountryRepository>();
            var target = new CountryService(mockCountryRepository.Object);

            mockCountryRepository.Setup(c => c.GetCountryById(countryId)).Returns(country);

            // Act
            var actual = target.GetCoutryById(countryId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Message, actual.Message);
            mockCountryRepository.Verify(c => c.GetCountryById(countryId), Times.Once);
        }

    }
}
