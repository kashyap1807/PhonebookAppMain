using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PhonebookAPI.Controllers;
using PhonebookAPI.Dtos;
using PhonebookAPI.Models;
using PhonebookAPI.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PhonebookAPITests.Controllers
{
    public class ContactControllerTests
    {
        //---------Get All Contacts ------------
        [Fact]
        public void GetAllContacts_ReturnsOkResponse_WhenSuccessIsTrue()
        {
            // Arrange
            int page = 1;
            int page_size = 10;
            string search_string = "";
            string sort_dir = "default";
            bool show_favourites = false;

            var contacts = new List<ContactDto>()
            {
                new ContactDto()
                {
                    contactId = 1,
                    FirstName = "Test",
                    LastName = "Test",
                    ContactNumber = "1234567890",
                },
                new ContactDto()
                {
                    contactId = 1,
                    FirstName = "Test",
                    LastName = "Test",
                    ContactNumber = "1234567890",
                },
            };

            var response = new ServiceResponse<IEnumerable<ContactDto>>()
            {
                Data = contacts,
                Total = 10,
                Success = true,
                Message = "",
            };

            var mockContactService = new Mock<IContactService>();
            var mockFileService = new Mock<IFileService>();
            mockContactService.Setup(c => c.GetPaginatedContactsStartingWithLetter(page, page_size, search_string, sort_dir, show_favourites)).Returns(response);

            var target = new ContactController(mockContactService.Object,mockFileService.Object);

            // Act
            var actual = target.GetAllPaginatedContactsStartingWithLetter(page, page_size, search_string, sort_dir, show_favourites) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetPaginatedContactsStartingWithLetter(page, page_size, search_string, sort_dir, show_favourites), Times.Once);
        }

        [Fact]
        public void GetAllContacts_ReturnsNotFound_WhenSuccessIsFalse()
        {
            // Arrange
            int page = 1;
            int page_size = 10;
            string search_string = "";
            string sort_dir = "default";
            bool show_favourites = false;

            var response = new ServiceResponse<IEnumerable<ContactDto>>()
            {
                Success = false,
                Message = "",
            };

            var mockContactService = new Mock<IContactService>();
            var mockFileService = new Mock<IFileService>();

            mockContactService.Setup(c => c.GetPaginatedContactsStartingWithLetter(page, page_size, search_string, sort_dir, show_favourites)).Returns(response);

            var target = new ContactController(mockContactService.Object, mockFileService.Object);

            // Act
            var actual = target.GetAllPaginatedContactsStartingWithLetter(page, page_size, search_string, sort_dir, show_favourites) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.NotFound, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetPaginatedContactsStartingWithLetter(page, page_size, search_string, sort_dir, show_favourites), Times.Once);
        }

        //-----------------Get Total Contacts----------------
        [Fact]
        public void GetTotalContacts_ReturnsOkResponse_WhenSuccessIsTrue()
        {
            // Arrange
            string search_string = "";
            bool show_favourites = false;

            var response = new ServiceResponse<int>()
            {
                Data = 10,
                Success = true,
                Message = "",
            };

            var mockContactService = new Mock<IContactService>();
            var mockFileService = new Mock<IFileService>();
            mockContactService.Setup(c => c.TotalContacts(search_string, show_favourites)).Returns(response);

            var target = new ContactController(mockContactService.Object,mockFileService.Object);

            // Act
            var actual = target.GetTotalContacts(search_string, show_favourites) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.TotalContacts(search_string, show_favourites), Times.Once);
        }

        [Fact]
        public void GetTotalContacts_ReturnsNotFound_WhenSuccessIsFalse()
        {
            // Arrange
            string search_string = "";
            bool show_favourites = false;

            var response = new ServiceResponse<int>()
            {
                Data = 0,
                Success = false,
                Message = "",
            };

            var mockContactService = new Mock<IContactService>();
            var mockFileService = new Mock<IFileService>();
            mockContactService.Setup(c => c.TotalContacts(search_string, show_favourites)).Returns(response);

            var target = new ContactController(mockContactService.Object, mockFileService.Object);

            // Act
            var actual = target.GetTotalContacts(search_string, show_favourites) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.NotFound, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.TotalContacts(search_string, show_favourites), Times.Once);
        }

        //---------------Get Contact By Id------------------
        [Fact]
        public void GetContactById_ReturnsOkResponse_WhenSuccessIsTrue()
        {
            // Arrange
            int contactId = 1;
            var contact = new ContactDto()
            {
                contactId = contactId,
                FirstName = "FirstName1",
                LastName = "LastName1",
                ContactNumber = "1234567890",
            };

            var response = new ServiceResponse<ContactDto>()
            {
                Data = contact,
                Success = true,
                Message = "",
            };

            var mockContactService = new Mock<IContactService>();
            var mockFileService = new Mock<IFileService>();
            mockContactService.Setup(c => c.GetContactById(contactId)).Returns(response);

            var target = new ContactController(mockContactService.Object, mockFileService.Object);

            // Act
            var actual = target.GetContactById(contactId) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetContactById(contactId), Times.Once);
        }

        [Fact]
        public void GetContactById_ReturnsNotFound_WhenSuccessIsFalse()
        {
            // Arrange
            int contactId = 1;

            var response = new ServiceResponse<ContactDto>()
            {
                Success = false,
                Message = "",
            };

            var mockContactService = new Mock<IContactService>();

            mockContactService.Setup(c => c.GetContactById(contactId)).Returns(response);
            var mockFileService = new Mock<IFileService>();
            var target = new ContactController(mockContactService.Object, mockFileService.Object);

            // Act
            var actual = target.GetContactById(contactId) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.NotFound, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetContactById(contactId), Times.Once);
        }

        //-----------Add Contacts-----
        [Fact]
        public void AddContact_ReturnsOkResponse_WhenSuccessIsTrue()
        {
            // Arrange
            Fixture fixture = new Fixture();
            var addContactDto = fixture.Build<AddContactDto>().Without(c => c.Image).Create();

            var response = new ServiceResponse<string>()
            {
                Success = true,
                Message = "",
            };

            var mockContactService = new Mock<IContactService>();
            var mockFileService = new Mock<IFileService>();

            mockContactService.Setup(c => c.AddContact(It.IsAny<Contact>())).Returns(response);

            var target = new ContactController(mockContactService.Object, mockFileService.Object);

            // Act
            var actual = target.AddContact(addContactDto) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.AddContact(It.IsAny<Contact>()), Times.Once);
        }

        [Fact]
        public void AddContact_ReturnsBadRequest_WhenSuccessIsFalse()
        {
            // Arrange
            Fixture fixture = new Fixture();
            var addContactDto = fixture.Build<AddContactDto>().Without(c => c.Image).Create();

            var response = new ServiceResponse<string>()
            {
                Success = false,
                Message = "Error",
            };

            var mockContactService = new Mock<IContactService>();
            var mockFileService = new Mock<IFileService>();

            mockContactService.Setup(c => c.AddContact(It.IsAny<Contact>())).Returns(response);

            var target = new ContactController(mockContactService.Object, mockFileService.Object);

            // Act
            var actual = target.AddContact(addContactDto) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.AddContact(It.IsAny<Contact>()), Times.Once);
        }

        [Fact]
        public void AddContact_ProcessesImage_WhenImageIsProvided()
        {
            // Arrange
            Fixture fixture = new Fixture();
            var addContactDto = fixture.Build<AddContactDto>()
                                       .With(c => c.Image, new FormFile(new MemoryStream(new byte[0]), 0, 0, "Data", "test.jpg"))
                                       .Create();

            var response = new ServiceResponse<string>()
            {
                Success = true,
                Message = "",
            };

            var mockContactService = new Mock<IContactService>();
            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(f => f.ToByteArray(It.IsAny<IFormFile>())).Returns(new byte[0]);

            mockContactService.Setup(c => c.AddContact(It.IsAny<Contact>())).Returns(response);

            var target = new ContactController(mockContactService.Object, mockFileService.Object);

            // Act
            var actual = target.AddContact(addContactDto) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.AddContact(It.Is<Contact>(contact => contact.FileName == "test.jpg")), Times.Once);
            mockFileService.Verify(f => f.ToByteArray(It.IsAny<IFormFile>()), Times.Once);
        }
        //------Update Contacts-----
        [Fact]
        public void UpdateContact_ReturnsOkResponse_WhenSuccessIsTrue()
        {
            // Arrange
            Fixture fixture = new Fixture();
            var contactDto = fixture.Build<UpdateContactDto>().Without(c => c.Image).Create();

            var response = new ServiceResponse<string>()
            {
                Success = true,
                Message = "",
            };

            var mockContactService = new Mock<IContactService>();

            mockContactService.Setup(c => c.ModifyContact(contactDto)).Returns(response);

            var mockFileService = new Mock<IFileService>();
            var target = new ContactController(mockContactService.Object, mockFileService.Object);

            // Act
            var actual = target.UpdateContact(contactDto) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.ModifyContact(contactDto), Times.Once);
        }

        [Fact]
        public void UpdateContact_ReturnsBadRequest_WhenSuccessIsFalse()
        {
            // Arrange
            Fixture fixture = new Fixture();
            var contactDto = fixture.Build<UpdateContactDto>().Without(c => c.Image).Create();

            var response = new ServiceResponse<string>()
            {
                Success = false,
                Message = "",
            };

            var mockContactService = new Mock<IContactService>();

            mockContactService.Setup(c => c.ModifyContact(contactDto)).Returns(response);
            var mockFileService = new Mock<IFileService>();
            var target = new ContactController(mockContactService.Object, mockFileService.Object);

            // Act
            var actual = target.UpdateContact(contactDto) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.ModifyContact(contactDto), Times.Once);
        }
        //-------------Remove Contacts-------------
        [Fact]
        public void DeleteContact_ReturnsOkResponse_WhenSuccessIsTrue()
        {
            // Arrange
            int contactId = 1;

            var response = new ServiceResponse<string>()
            {
                Success = true,
                Message = "",
            };

            var mockContactService = new Mock<IContactService>();

            mockContactService.Setup(c => c.RemoveContact(contactId)).Returns(response);

            var mockFileService = new Mock<IFileService>();
            var target = new ContactController(mockContactService.Object, mockFileService.Object);

            // Act
            var actual = target.RemoveContact(contactId) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.RemoveContact(contactId), Times.Once);
        }

        [Fact]
        public void DeleteContact_ReturnsBadRequest_WhenSuccessIsFalse()
        {
            // Arrange
            int contactId = 1;

            var response = new ServiceResponse<string>()
            {
                Success = false,
                Message = "",
            };

            var mockContactService = new Mock<IContactService>();

            mockContactService.Setup(c => c.RemoveContact(contactId)).Returns(response);

            var mockFileService = new Mock<IFileService>();
            var target = new ContactController(mockContactService.Object, mockFileService.Object);

            // Act
            var actual = target.RemoveContact(contactId) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.RemoveContact(contactId), Times.Once);
        }

        [Fact]
        public void DeleteContact_ReturnsBadRequest_WhenIdIsLessThanOne()
        {
            // Arrange
            int contactId = 0;

            var mockContactService = new Mock<IContactService>();

            var mockFileService = new Mock<IFileService>();
            var target = new ContactController(mockContactService.Object, mockFileService.Object);

            // Act
            var actual = target.RemoveContact(contactId) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
            Assert.NotNull(actual.Value);
        }
    }
}
