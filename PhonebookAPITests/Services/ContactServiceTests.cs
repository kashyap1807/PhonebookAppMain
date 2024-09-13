using Microsoft.AspNetCore.Http;
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
    public class ContactServiceTests
    {
        //---------------GetAllContactsByPagination-----------
        [Fact]
        public void GetAllContacts_ReturnErrorMessage_whenNoContactExist()
        {
            //Arrange
            IEnumerable<Contact> contacts = null;
            var response = new ServiceResponse<IEnumerable<Contact>>()
            {
                Data = contacts,
                Success = false,
                Message = "No record found",
            };
            var mockContactRepository = new Mock<IContactRepository>();
            var mockFileService = new Mock<IFileService>();
            mockContactRepository.Setup(c=>c.GetPaginatedContactsStartingWithLetter(1,2,"","",false)).Returns(contacts);
            var target = new ContactService(mockContactRepository.Object, mockFileService.Object);

            //Act
            var actual = target.GetPaginatedContactsStartingWithLetter(1,2,"","",false);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Message, actual.Message);
            mockContactRepository.Verify(c => c.GetPaginatedContactsStartingWithLetter(1, 2, "", "", false),Times.Once);

        }

        [Fact]
        public void GetAllContacts_ReturnContacts_whenContactExist()
        {
            //Arrange
            var contacts = new List<Contact>
            {
                new Contact
                {
                    ContactId = 1,
                    FirstName = "FirstName1",
                    LastName = "LastName1",
                    Email = "email@gmail.com",
                    Company = "FCD",
                    ContactNumber = "1234567890",
                    FileName = "sampleImg.png",
                    CountryId = 1,
                    StateId = 1,
                    Gender = "M",
                    IsFavourite = true,
                    Image = new byte[123],
                    Country = new Country()
                    {
                        CountryId = 1,
                        CountryName = "Country1",
                    },
                    State = new State()
                    {
                        StateId = 1,
                        StateName = "State1",
                    }
                }
                
            };
            var response = new ServiceResponse<IEnumerable<Contact>>()
            {
                Data = contacts,
                Success = true,
                Message = "Success",
            };
            var mockContactRepository = new Mock<IContactRepository>();
            var mockFileService = new Mock<IFileService>();
            mockContactRepository.Setup(c => c.GetPaginatedContactsStartingWithLetter(1, 2, "", "", false)).Returns(contacts);
            var target = new ContactService(mockContactRepository.Object, mockFileService.Object);

            //Act
            var actual = target.GetPaginatedContactsStartingWithLetter(1, 2, "", "", false);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Message, actual.Message);
            mockContactRepository.Verify(c => c.GetPaginatedContactsStartingWithLetter(1, 2, "", "", false), Times.Once);

        }

        //---------------Get Contact By Id----------

        [Fact]
        public void GetContactById_ReturnContact_WhenContactExists()
        {
            //Arrange
            var contact = new Contact
            {
                ContactId = 1,
                FirstName = "FirstName1",
                LastName = "LastName1",
                Email = "email@gmail.com",
                Company = "FCD",
                ContactNumber = "1234567890",
                FileName = "sampleImg.png",
                CountryId = 1,
                StateId = 1,
                Gender = "M",
                IsFavourite = true,
                Image = new byte[123],
                Country = new Country(){
                    CountryId=1,
                    CountryName="Country1",
                },
                State = new State()
                {
                    StateId=1,
                    StateName="State1",
                }
            };
            var response = new ServiceResponse<Contact>()
            {
                Data = contact,
                Success = true,
                Message = " ",
            };  

            var mockContactRepository = new Mock<IContactRepository>();
            var mockFileService = new Mock<IFileService>();

            mockContactRepository.Setup(c => c.GetContact(contact.ContactId)).Returns(contact);

            var target = new ContactService(mockContactRepository.Object, mockFileService.Object);

            //Act
            var actual = target.GetContactById(contact.ContactId);

            //Assert
            Assert.NotNull(actual);
            mockContactRepository.Verify(c => c.GetContact(contact.ContactId),Times.Once);


        }

        //--------------Total Contacts---------------
        [Fact]
        public void TotalContacts_ReturnsResponse_WhenContactExists()
        {
            //Arrange
            var contacts = new List<Contact>
            {
                new Contact
                {
                    ContactId = 1,
                    FirstName = "FirstName1",
                    LastName = "LastName1",
                    Email = "email@gmail.com",
                    Company = "FCD",
                    ContactNumber = "1234567890",
                    FileName = "sampleImg.png",
                    CountryId = 1,
                    StateId = 1,
                    Gender = "M",
                    IsFavourite = true,
                    Image = new byte[123],
                    Country = new Country()
                    {
                        CountryId = 1,
                        CountryName = "Country1",
                    },
                    State = new State()
                    {
                        StateId = 1,
                        StateName = "State1",
                    }
                }

            };
            var response = new ServiceResponse<IEnumerable<Contact>>()
            {
                Data = contacts,
                Success = true,
                Message = null,
                
            };

            var mockContactRepository = new Mock<IContactRepository>();
            var mockFileService = new Mock<IFileService>();
            mockContactRepository.Setup(c => c.TotalContacts("", false)).Returns(1);
            var target = new ContactService(mockContactRepository.Object, mockFileService.Object);

            //Act
            var actual = target.TotalContacts("", false);

            //Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            Assert.Equal(1, actual.Data);
            mockContactRepository.Verify(c => c.TotalContacts("", false),Times.Once);

        }

        //--------Add Contacts----------
        [Fact]
        public void AddContact_ReturnsFailure_WhenContactIsNull()
        {
            // Arrange
            Contact contact = null;

            var response = new ServiceResponse<IEnumerable<string>>()
            {
                Data = null,
                Success = true,
                Message = "Something went wrong. Please try after sometime.",

            };

            var mockContactRepository = new Mock<IContactRepository>();
            var mockFileService = new Mock<IFileService>();
            var target = new ContactService(mockContactRepository.Object,mockFileService.Object);

            // Act
            var actual = target.AddContact(contact);

            // Assert
            Assert.False(actual.Success);
            Assert.Equal(response.Message,actual.Message);
        }

        [Fact]
        public void AddContact_ReturnFailure_WhenContactExists()
        {
            //Arrange
            var contact = new Contact
            {
                ContactId = 1,
                ContactNumber = "1234567890"
            };

            var response = new ServiceResponse<Contact>()
            {
                Data= contact,
                Success = false,
                Message= "Contact already exists.",
            };

            var mockContactRepository = new Mock<IContactRepository>();
            var mockFileService = new Mock<IFileService>();
            mockContactRepository.Setup(c => c.ContactExists(contact.ContactId, contact.ContactNumber)).Returns(true);
            var target =  new ContactService(mockContactRepository.Object,mockFileService.Object);

            //Act
            var actual = target.AddContact(contact);
            
            //Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(response.Message, actual.Message);
            mockContactRepository.Verify(c => c.ContactExists(contact.ContactId, contact.ContactNumber),Times.Once);

        }

        [Fact]
        public void AddContacts_ReturnSuccess_WhenContactIsAdded()
        {
            //Arrange
            var contact = new Contact
            {
                ContactId = 1,
                FirstName = "FirstName1",
                LastName = "LastName1",
                Email = "email@gmail.com",
                Company = "FCD",
                ContactNumber = "1234567890",
                FileName = "sampleImg.png",
                CountryId = 1,
                StateId = 1,
                Gender = "M",
                IsFavourite = true,
                Image = new byte[123],
                Country = new Country()
                {
                    CountryId = 1,
                    CountryName = "Country1",
                },
                State = new State()
                {
                    StateId = 1,
                    StateName = "State1",
                }
            };

            var response = new ServiceResponse<Contact>()
            {
                Data= contact,
                Success =true,
                Message = "Contact saved successfully."
            };

            var mockContactRepository = new Mock<IContactRepository>();
            var mockFileService = new Mock<IFileService>();
            mockContactRepository.Setup(c => c.ContactExists(contact.ContactId, contact.ContactNumber)).Returns(false);
            mockContactRepository.Setup(c => c.InsertContact(contact)).Returns(true);
            var target = new ContactService(mockContactRepository.Object,mockFileService.Object);

            //Act
            var actual = target.AddContact(contact);

            //Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            Assert.Equal(response.Success, actual.Success);
            mockContactRepository.Verify(c => c.ContactExists(contact.ContactId, contact.ContactNumber),Times.Once);
            mockContactRepository.Verify(c => c.InsertContact(contact),Times.Once);
        }

        [Fact]
        public void AddContact_ReturnFailure_WhenInsertFails()
        {
            //Arrange
            var contact = new Contact
            {
                ContactId = 1,
                FirstName = "FirstName1",
                LastName = "LastName1",
                Email = "email@gmail.com",
                Company = "FCD",
                ContactNumber = "1234567890",
                FileName = "sampleImg.png",
                CountryId = 1,
                StateId = 1,
                Gender = "M",
                IsFavourite = true,
                Image = new byte[123],
                Country = new Country()
                {
                    CountryId = 1,
                    CountryName = "Country1",
                },
                State = new State()
                {
                    StateId = 1,
                    StateName = "State1",
                }
            };

            var response = new ServiceResponse<Contact>()
            {
                Data = contact,
                Success = false,
                Message = "Something went wrong. Please try after sometime."
            };

            var mockContactRepository = new Mock<IContactRepository>();
            var mockFileService = new Mock<IFileService>();
            mockContactRepository.Setup(c => c.ContactExists(contact.ContactId, contact.ContactNumber)).Returns(false);
            mockContactRepository.Setup(c => c.InsertContact(contact)).Returns(false);
            var target = new ContactService(mockContactRepository.Object, mockFileService.Object);

            //Act
            var actual = target.AddContact(contact);

            //Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(response.Message, actual.Message);
            mockContactRepository.Verify(c => c.ContactExists(contact.ContactId, contact.ContactNumber), Times.Once);
            mockContactRepository.Verify(c => c.InsertContact(contact), Times.Once);
        }


        //------------Update Contacts-----------
        [Fact]
        public void ModifyContact_ReturnFailure_WhenContactIsNull()
        {
            // Arrange
            UpdateContactDto contact = null;

            var response = new ServiceResponse<UpdateContactDto>()
            {
                Data = contact,
                Success = false,
                Message = "Something went wrong. Please try after sometime.",

            };

            var mockContactRepository = new Mock<IContactRepository>();
            var mockFileService = new Mock<IFileService>();
            var target = new ContactService(mockContactRepository.Object, mockFileService.Object);

            // Act
            var actual = target.ModifyContact(contact);

            // Assert
            Assert.False(actual.Success);
            Assert.Equal(response.Message, actual.Message);
        }

        [Fact]
        public void ModifyContact_ReturnFailure_WhenCOntactAlreadyExists()
        {
            //Arrange
            var contact = new UpdateContactDto
            {
                contactId = 1,
                ContactNumber = "1234567890"
            };

            var response = new ServiceResponse<UpdateContactDto>()
            {
                Data = contact,
                Success = false,
                Message = "Contact already exists.",
            };

            var mockContactRepository = new Mock<IContactRepository>();
            var mockFileService = new Mock<IFileService>();
            mockContactRepository.Setup(c => c.ContactExists(contact.contactId, contact.ContactNumber)).Returns(true);
            var target = new ContactService(mockContactRepository.Object, mockFileService.Object);


            //Act
            var actual = target.ModifyContact(contact);

            //Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(response.Message, actual.Message);
            mockContactRepository.Verify(c => c.ContactExists(contact.contactId, contact.ContactNumber),Times.Once);

        }

        [Fact]
        public void ModifyContact_ReturnsFailure_WhenContactToModifyDoesNotExist()
        {
            //Arrange
            var contact = new UpdateContactDto
            {
                contactId = 1,
                ContactNumber = "1234567890"
            };

            var mockContactRepository = new Mock<IContactRepository>();
            var mockFileService = new Mock<IFileService>();
            mockContactRepository.Setup(c => c.ContactExists(contact.contactId, contact.ContactNumber)).Returns(false);
            mockContactRepository.Setup(c => c.GetContact(contact.contactId)).Returns((Contact)null);
            var target = new ContactService(mockContactRepository.Object, mockFileService.Object);

            //Act
            var actual = target.ModifyContact(contact);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal("Something went wrong. Please try after sometime.", actual.Message);
            mockContactRepository.Verify(c => c.ContactExists(contact.contactId, contact.ContactNumber),Times.Once);
            mockContactRepository.Verify(c => c.GetContact(contact.contactId),Times.Once);
        }

        [Fact]
        public void ModifyContact_ReturnSuccess_WhenContactIsModified()
        {
            //Arrange
            var contact = new UpdateContactDto
            {
                contactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Company = "Company",
                ContactNumber = "1234567890",
                CountryId = 1,
                StateId = 1,
                Gender = "M",
                IsFavourite = true,
                Image = new FormFile(new MemoryStream(new byte[1]), 0, 1, "image", "image.jpg")
            };

            var response = new ServiceResponse<UpdateContactDto>()
            {
                Data = contact,
                Success = true,
                Message = "Contact updated successfully.",
            };


            var existingContact = new Contact
            {
                ContactId = contact.contactId
            };
            var mockContactRepository = new Mock<IContactRepository>();
            var mockFileService = new Mock<IFileService>();

            mockContactRepository.Setup(c => c.ContactExists(contact.contactId, contact.ContactNumber)).Returns(false);
            mockContactRepository.Setup(c=>c.GetContact(contact.contactId)).Returns(existingContact);
            mockContactRepository.Setup(c => c.UpdateContact(It.IsAny<Contact>())).Returns(true);

            mockFileService.Setup(f=>f.ToByteArray(contact.Image)).Returns(new byte[1]);

            var target = new ContactService(mockContactRepository.Object, mockFileService.Object);

            //Act
            var actual = target.ModifyContact(contact);

            //Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            Assert.Equal(response.Message, actual.Message);

            mockContactRepository.Verify(c => c.ContactExists(contact.contactId, contact.ContactNumber),Times.Once);
            mockContactRepository.Verify(c => c.GetContact(contact.contactId),Times.Once);
            mockContactRepository.Verify(c => c.UpdateContact(It.IsAny<Contact>()),Times.Once);
        }

        [Fact]
        public void ModifyContact_ReturnsFailure_WhenModifyContactFail()
        {
            //Arrange
            var contact = new UpdateContactDto
            {
                contactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Company = "Company",
                ContactNumber = "1234567890",
                CountryId = 1,
                StateId = 1,
                Gender = "M",
                IsFavourite = true,
                Image = new FormFile(new MemoryStream(new byte[1]), 0, 1, "image", "image.jpg")
            };

            var response = new ServiceResponse<UpdateContactDto>()
            {
                Data = contact,
                Success = false,
                Message = "Something went wrong. Please try after sometime.",
            };


            var existingContact = new Contact
            {
                ContactId = contact.contactId
            };
            var mockContactRepository = new Mock<IContactRepository>();
            var mockFileService = new Mock<IFileService>();

            mockContactRepository.Setup(c => c.ContactExists(contact.contactId, contact.ContactNumber)).Returns(false);
            mockContactRepository.Setup(c => c.GetContact(contact.contactId)).Returns(existingContact);
            mockContactRepository.Setup(c => c.UpdateContact(It.IsAny<Contact>())).Returns(false);

            mockFileService.Setup(f => f.ToByteArray(contact.Image)).Returns(new byte[1]);

            var target = new ContactService(mockContactRepository.Object, mockFileService.Object);

            //Act
            var actual = target.ModifyContact(contact);

            //Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(response.Message, actual.Message);

            mockContactRepository.Verify(c => c.ContactExists(contact.contactId, contact.ContactNumber), Times.Once);
            mockContactRepository.Verify(c => c.GetContact(contact.contactId), Times.Once);
            mockContactRepository.Verify(c => c.UpdateContact(It.IsAny<Contact>()), Times.Once);
        }

        //--------------Delete Contact-----------
        [Fact]
        public void RemoveContact_ReturnFailure_WhenRemoveFail()
        {
            //Arrange
            var contactId = 1;

            var response = new ServiceResponse<Contact>
            {
                Data = {},
                Success=false,
                Message= "Something went wrong, please try after sometime.",
            };
            var mockContactRepository = new Mock<IContactRepository>();
            var mockFileService = new Mock<IFileService>();
            mockContactRepository.Setup(c => c.DeleteContact(contactId)).Returns(false);
            var target = new ContactService(mockContactRepository.Object, mockFileService.Object);

            //Act
            var actual = target.RemoveContact(contactId);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Message, actual.Message);
            mockContactRepository.Verify(c => c.DeleteContact(contactId),Times.Once);

        }
        [Fact]
        public void RemoveContact_ReturnFailure_WhenRecordNotFound()
        {
            // Arrange
            var response = new ServiceResponse<string>()
            {
                Data = { },
                Success = false,
                Message = "Something went wrong, please try after sometime.",
            };
            var mockContactRepository = new Mock<IContactRepository>();
            var mockFileService = new Mock<IFileService>();
            var contactId = -1;
            var target = new ContactService(mockContactRepository.Object,mockFileService.Object);

            //Act
            var actual = target.RemoveContact(contactId);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Message, actual.Message);

        }

        [Fact]
        public void DeleteContact_ReturnsSuccessMessage_WhenDeletedSuccessfully()
        {
            // Arrange
            var response = new ServiceResponse<string>()
            {
                Data = { },
                Success = false,
                Message = "Contact deleted successfully.",
            };
            var mockContactRepository = new Mock<IContactRepository>();
            var mockFileService = new Mock<IFileService>();
            var contactId = 1;
            var target = new ContactService(mockContactRepository.Object, mockFileService.Object);

            mockContactRepository.Setup(c => c.DeleteContact(contactId)).Returns(true);

            //Act
            var actual = target.RemoveContact(contactId);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Message, actual.Message);
            mockContactRepository.Verify(c => c.DeleteContact(contactId),Times.Once);
        }
    }
}
