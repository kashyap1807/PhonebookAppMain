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
    public class StateServiceTests
    {
        [Fact]
        public void GetState_ReturnFailure_WhenCOuntryIdNotFound()
        {
            //Arrange
            var countryId = 1;
            var state = new List<State>();

            var response = new ServiceResponse<List<State>>
            {
                Data=state,
                Success=false,
                Message= "No record found",
            };

            var mockStateRepository = new Mock<IStateRepository>();
            mockStateRepository.Setup(s => s.GetStatesBytCountryId(countryId)).Returns(state);
            var target  = new StateService(mockStateRepository.Object);
            //Act
            var actual = target.GetStateByCountryId(countryId);

            //Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(response.Message, actual.Message);
            mockStateRepository.Verify(s => s.GetStatesBytCountryId(countryId),Times.Once);

        }
        [Fact]
        public void GetState_ReturnSuccess_WhenCountryIdFound()
        {
            // Arrange
            var stateId = 1;
            var state = new List<State>
            {
                new State
                {
                    StateId=1,
                    StateName="State1",
                    CountryId=1,
                }
                
                
            };
            var response = new ServiceResponse<List<State>>()
            {
                Data = state,
                Success = true,
                Message = null,
            };
            var mockStateRepository = new Mock<IStateRepository>();
            var target = new StateService(mockStateRepository.Object);

            mockStateRepository.Setup(c => c.GetStatesBytCountryId(stateId)).Returns(state);

            // Act
            var actual = target.GetStateByCountryId(stateId);

            // Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            Assert.Equal(response.Message, actual.Message);
            mockStateRepository.Verify(c => c.GetStatesBytCountryId(stateId), Times.Once);
        }
    }
}
