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
    public class StateControllerTests
    {
        //----------------------- GetStatesByCountryId--------------

        [Fact]
        public void GetStatesByCountryId_ReturnsOkResponse_WhenSuccessIsTrue()
        {
            // Arrange
            int countryId = 1;
            var states = new List<StateDto>()
            {
                new StateDto()
                {
                    StateId = 1,
                    StateName = "State 1",
                    CountryId = 1,
                },
                new StateDto()
                {
                    StateId = 2,
                    StateName = "State 2",
                    CountryId = 1,
                },
            };

            var response = new ServiceResponse<IEnumerable<StateDto>>()
            {
                Data = states,
                Success = true,
                Message = ""
            };

            var mockStateService = new Mock<IStateService>();

            mockStateService.Setup(c => c.GetStateByCountryId(countryId)).Returns(response);

            var target = new StateController(mockStateService.Object);

            // Act
            var actual = target.GetStateByCountryId(countryId) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockStateService.Verify(c => c.GetStateByCountryId(countryId), Times.Once);
        }

        [Fact]
        public void GetStatesByCountryId_ReturnsNotFound_WhenSuccessIsFalse()
        {
            // Arrange
            int countryId = 1;

            var response = new ServiceResponse<IEnumerable<StateDto>>()
            {
                Success = false,
                Message = ""
            };

            var mockStateService = new Mock<IStateService>();

            mockStateService.Setup(c => c.GetStateByCountryId(countryId)).Returns(response);

            var target = new StateController(mockStateService.Object);

            // Act
            var actual = target.GetStateByCountryId(countryId) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.NotFound, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockStateService.Verify(c => c.GetStateByCountryId(countryId), Times.Once);
        }

       
    }
}
