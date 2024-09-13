using Microsoft.AspNetCore.Mvc;
using Moq;
using PhonebookAPI.Controllers;
using PhonebookAPI.Dtos;
using PhonebookAPI.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PhonebookAPITests.Controllers
{
    public class CountryControllerTests
    {
        //-------------Get All Country-----
        [Fact]
        public void GetAllCountries_ReturnsOkResponse_WhenSuccessIsTrue()
        {
            // Arrange
            var countries = new List<CountryDto>()
            {
                new CountryDto()
                {
                    CountryId = 1,
                    CountryName = "Country 1",
                },
                new CountryDto()
                {
                    CountryId = 2,
                    CountryName = "Country 2",
                },
            };

            var response = new ServiceResponse<IEnumerable<CountryDto>>()
            {
                Data = countries,
                Success = true,
                Message = ""
            };

            var mockCountryService = new Mock<ICountryService>();

            mockCountryService.Setup(c => c.GetAllCountry()).Returns(response);

            var target = new CountryController(mockCountryService.Object);

            // Act
            var actual = target.GetAllCountry() as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockCountryService.Verify(c => c.GetAllCountry(), Times.Once);
        }

        [Fact]
        public void GetAllCountries_ReturnsNotFound_WhenSuccessIsFalse()
        {
            // Arrange
            var response = new ServiceResponse<IEnumerable<CountryDto>>()
            {
                Success = false,
                Message = ""
            };

            var mockCountryService = new Mock<ICountryService>();

            mockCountryService.Setup(c => c.GetAllCountry()).Returns(response);

            var target = new CountryController(mockCountryService.Object);

            // Act
            var actual = target.GetAllCountry() as NotFoundObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.NotFound, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockCountryService.Verify(c => c.GetAllCountry(), Times.Once);
        }

        //---------------GetCountryById-------

        [Fact]
        public void GetCountryById_ReturnsOkResponse_WhenSuccessIsTrue()
        {
            // Arrange
            int countryId = 1;
            var country = new CountryDto()
            {
                CountryId = countryId,
                CountryName = "Country 1",
            };

            var response = new ServiceResponse<CountryDto>()
            {
                Data = country,
                Success = true,
                Message = ""
            };

            var mockCountryService = new Mock<ICountryService>();

            mockCountryService.Setup(c => c.GetCoutryById(countryId)).Returns(response);

            var target = new CountryController(mockCountryService.Object);

            // Act
            var actual = target.GetCountryById(countryId) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockCountryService.Verify(c => c.GetCoutryById(countryId), Times.Once);
        }

        [Fact]
        public void GetCountryById_ReturnsNotFound_WhenSuccessIsFalse()
        {
            // Arrange
            int countryId = 1;

            var response = new ServiceResponse<CountryDto>()
            {
                Success = false,
                Message = ""
            };

            var mockCountryService = new Mock<ICountryService>();

            mockCountryService.Setup(c => c.GetCoutryById(countryId)).Returns(response);

            var target = new CountryController(mockCountryService.Object);

            // Act
            var actual = target.GetCountryById(countryId) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.NotFound, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockCountryService.Verify(c => c.GetCoutryById(countryId), Times.Once);
        }
    }
}
