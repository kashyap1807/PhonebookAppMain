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
    public class ContactRepositoryTests
    {
        //-----------Get Contact with Id-------------
        [Fact]
        public void GetContactById_ReturnContact_WhenIdFound()
        {
            //Arrange
            var id = 1;
            var contact = new Contact { ContactId = id ,FirstName = "Contact1"};
            var contacts = new List<Contact> { contact }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();

            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.SetupGet(c => c.Contacts).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.GetContact(id);

            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);

        }


        [Fact]
        public void GetContactById_ReturnNull_WhenIdNotFound()
        {
            //Arrange
            var id = 0;
            
            var contacts = new List<Contact> {  }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();

            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.SetupGet(c => c.Contacts).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.GetContact(id);

            //Assert
            Assert.Null(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }

        //---------------Insert Contacts---------------
        [Fact]
        public void InsertContact_ReturnsTrue()
        {
            // Arrange
            var mockDbSet = new Mock<DbSet<Contact>>();
            var mockAppDbContext = new Mock<IAppDbContext>();
            var contact = new Contact { ContactId = 1, FirstName = "ContactFirstName 1" ,LastName="ContactLastName 1"};
            mockAppDbContext.SetupGet(c => c.Contacts).Returns(mockDbSet.Object);
            mockAppDbContext.Setup(c => c.SaveChanges()).Returns(1);
            var target = new ContactRepository(mockAppDbContext.Object);

            // Act
            var actual = target.InsertContact(contact);

            // Assert
            Assert.True(actual);
            mockDbSet.Verify(c => c.Add(contact), Times.Once);
            mockAppDbContext.Verify(p => p.SaveChanges(), Times.Once);
        }

        [Fact]
        public void InsertContact_ReturnsFalse_WhenContactIsNull()
        {
            // Arrange
            var mockAppDbContext = new Mock<IAppDbContext>();
            Contact contact = null;
            var target = new ContactRepository(mockAppDbContext.Object);

            // Act
            var actual = target.InsertContact(contact);

            // Assert
            Assert.False(actual);
        }

        //-------------Update Contact-----------------
        [Fact]
        public void UpdateContact_ReturnTrue()
        {
            //Arrange
            var contacts = new Contact
            {
                ContactId = 1,
                FirstName = "ContactFirstName 1",
                LastName = "ContactLastName 1"
            };
            var mockDbset = new Mock<DbSet<Contact>>();
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.SetupGet(p => p.Contacts).Returns(mockDbset.Object);
            mockAppDbContext.Setup(p => p.SaveChanges()).Returns(1);
            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.UpdateContact(contacts);

            //Assert
            Assert.True(actual);
            mockAppDbContext.VerifyGet(p => p.Contacts, Times.Once);
            mockAppDbContext.Verify(p => p.SaveChanges(), Times.Once);
        }

        [Fact]
        public void UpdateProduct_ReturnFalse()
        {
            //Arrange

            var mockDbset = new Mock<DbSet<Contact>>();
            var mockAppDbContext = new Mock<IAppDbContext>();

            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.UpdateContact(null);

            //Assert
            Assert.False(actual);
        }

        //-------------------Delete Contact----------------
        [Fact]
        public void DeleteProduct_ReturnTrue()
        {
            //Arrange
            var id = 1;
            var contact = new Contact { ContactId = id, FirstName = "ContactFirstName 1" };
            var contacts = new List<Contact> { contact };
            var mockDbSet = new Mock<DbSet<Contact>>();
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockDbSet.Setup(c => c.Find(id)).Returns<object[]>(ids => contacts.Find(c => c.ContactId == (int)ids[0]));
            mockAppDbContext.SetupGet(c => c.Contacts).Returns(mockDbSet.Object);
            mockAppDbContext.Setup(c => c.SaveChanges()).Returns(1);
            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.DeleteContact(id);

            //Assert
            Assert.True(actual);
            mockDbSet.Verify(c => c.Find(id), Times.Once);
            mockDbSet.Verify(c => c.Remove(contact), Times.Once);
            mockAppDbContext.Verify(c => c.Contacts, Times.Exactly(2));
            mockAppDbContext.Verify(c => c.SaveChanges(), Times.Once);

        }


        [Fact]
        public void DeleteProduct_ReturnFalse()
        {
            //Arrange
            var id = 0;
            var contact = new Contact { ContactId = 1, FirstName = "ContactFirstName 1" };
            var contacts = new List<Contact> { contact };
            var mockDbSet = new Mock<DbSet<Contact>>();
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockDbSet.Setup(c => c.Find(id)).Returns<object[]>(ids => contacts.Find(c => c.ContactId == (int)ids[0]));
            mockAppDbContext.SetupGet(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.DeleteContact(id);

            //Assert
            Assert.False(actual);
            mockDbSet.Verify(c => c.Find(id), Times.Once);
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);

        }

        //-------------Contact Already Exists---------
        [Fact]
        public void ContactExists_ReturnContact()
        {
            //Arrange
            var contact = new Contact()
            {
                ContactNumber = "9099876543"
            };
            var contacts = new List<Contact>()
            {
                contact
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();

            // The Expression property represents the LINQ expression tree associated with the IQueryable collection.
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);

            // The Provider property is used by LINQ query execution.
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);

            // By setting up these properties, we ensure that when methods or properties of the DbSet<Category> are invoked in the unit test,
            // they behave as expected, providing access to the LINQ query provider and expression. This allows us to mimic the behavior of a
            // real database context in our unit tests.

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.SetupGet(c => c.Contacts).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.ContactExists(contact.ContactNumber);

            //Assert
            Assert.True(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Once);
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);

        }
        [Fact]
        public void ContactExists_ReturnNull()
        {
            //Arrange
            var contact = new Contact()
            {
                ContactNumber = "1234567890"
                
            };
            var contacts = new List<Contact>()
            {
                new Contact{ContactNumber="1234554321"}
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();

            // The Expression property represents the LINQ expression tree associated with the IQueryable collection.
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);

            // The Provider property is used by LINQ query execution.
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);

            // By setting up these properties, we ensure that when methods or properties of the DbSet<Category> are invoked in the unit test,
            // they behave as expected, providing access to the LINQ query provider and expression. This allows us to mimic the behavior of a
            // real database context in our unit tests.

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.SetupGet(c => c.Contacts).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.ContactExists(contact.ContactNumber);

            //Assert
            Assert.False(actual);
            
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Once);
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);

        }

        [Fact]
        public void ContactExists2_ReturnContact()
        {
            //Arrange
            var contact = new Contact()
            {
                ContactId=2,
                ContactNumber = "9099876543"
            };
            var contacts = new List<Contact>()
            {
                contact
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();

            // The Expression property represents the LINQ expression tree associated with the IQueryable collection.
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);

            // The Provider property is used by LINQ query execution.
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);

            // By setting up these properties, we ensure that when methods or properties of the DbSet<Category> are invoked in the unit test,
            // they behave as expected, providing access to the LINQ query provider and expression. This allows us to mimic the behavior of a
            // real database context in our unit tests.

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.SetupGet(c => c.Contacts).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.ContactExists(1,contact.ContactNumber);

            //Assert
            Assert.True(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Once);
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);

        }
        [Fact]
        public void ContactExists2_ReturnNull()
        {
            //Arrange
            var contact = new Contact()
            {
                ContactId=1,
                ContactNumber = "1234567890"

            };
            var contacts = new List<Contact>()
            {
                new Contact{ContactId = 2, ContactNumber="1234554321"}
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();

            // The Expression property represents the LINQ expression tree associated with the IQueryable collection.
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);

            // The Provider property is used by LINQ query execution.
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);

            // By setting up these properties, we ensure that when methods or properties of the DbSet<Category> are invoked in the unit test,
            // they behave as expected, providing access to the LINQ query provider and expression. This allows us to mimic the behavior of a
            // real database context in our unit tests.

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.SetupGet(c => c.Contacts).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.ContactExists(contact.ContactId,contact.ContactNumber);

            //Assert
            Assert.False(actual);

            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Once);
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);

        }

        //----------Total Contacts---------------
        [Fact]
        public void TotalContacts_ReturnsCorrectCount_WhenNoSearchStringAndNotFavourites()
        {
            // Arrange
            var contacts = new List<Contact>
            {
                new Contact { ContactId = 1, FirstName = "FirstName1", LastName = "LastName1", ContactNumber = "1234567890", IsFavourite = false },
                new Contact { ContactId = 2, FirstName = "FirstName2", LastName = "LastName2", ContactNumber = "0987654321", IsFavourite = true }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.ElementType).Returns(contacts.ElementType);
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.GetEnumerator()).Returns(contacts.GetEnumerator());

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockAppDbContext.Object);

            // Act
            var actual = target.TotalContacts(null, false);

            // Assert
            Assert.Equal(2, actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(m => m.Provider,Times.Exactly(3));
            mockDbSet.As<IQueryable<Contact>>().Verify(m => m.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(m => m.ElementType, Times.Exactly(0));
            mockDbSet.As<IQueryable<Contact>>().Verify(m => m.GetEnumerator(), Times.Exactly(0));
        }

        [Fact]
        public void TotalContacts_ReturnsCorrectCount_WhenFavouritesOnly()
        {
            // Arrange
            var contacts = new List<Contact>
            {
                new Contact { ContactId = 1, FirstName = "FirstName1", LastName = "LastName1", ContactNumber = "1234567890", IsFavourite = false },
                new Contact { ContactId = 2, FirstName = "FirstName2", LastName = "LastName2", ContactNumber = "6789012345", IsFavourite = true }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.ElementType).Returns(contacts.ElementType);
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.GetEnumerator()).Returns(contacts.GetEnumerator());

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockAppDbContext.Object);

            // Act
            var actual = target.TotalContacts(null, true);

            // Assert
            Assert.Equal(1, actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(m => m.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<Contact>>().Verify(m => m.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(m => m.ElementType, Times.Exactly(0));
            mockDbSet.As<IQueryable<Contact>>().Verify(m => m.GetEnumerator(), Times.Exactly(0));
        }

        [Fact]
        public void TotalContacts_ReturnsCorrectCount_WithSearchString()
        {
            // Arrange
            var contacts = new List<Contact>
            {
                new Contact { ContactId = 1, FirstName = "Ram", LastName = "Raghu", ContactNumber = "1234567890", IsFavourite = false },
                new Contact { ContactId = 2, FirstName = "Shyam", LastName = "Yadav", ContactNumber = "6789012345", IsFavourite = false },
                new Contact { ContactId = 3, FirstName = "Radha", LastName = "krishna", ContactNumber = "5432678901", IsFavourite = true }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.ElementType).Returns(contacts.ElementType);
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.GetEnumerator()).Returns(contacts.GetEnumerator());

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockAppDbContext.Object);

            // Act
            var actual = target.TotalContacts("Shyam", false);

            // Assert
            Assert.Equal(1, actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(m => m.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<Contact>>().Verify(m => m.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(m => m.ElementType, Times.Exactly(0));
            mockDbSet.As<IQueryable<Contact>>().Verify(m => m.GetEnumerator(), Times.Exactly(0));

        }

        [Fact]
        public void TotalContacts_ReturnsCorrectCount_WithSearchStringAndFavourites()
        {
            // Arrange
            var contacts = new List<Contact>
        {
            new Contact { ContactId = 1, FirstName = "FirstName1", LastName = "LastName1", ContactNumber = "1234567890", IsFavourite = false },
            new Contact { ContactId = 2, FirstName = "FirstName2", LastName = "LastName2", ContactNumber = "6789012345", IsFavourite = true },
            new Contact { ContactId = 3, FirstName = "FirstName3", LastName = "LastName3", ContactNumber = "5432167890", IsFavourite = true }
        }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.ElementType).Returns(contacts.ElementType);
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.GetEnumerator()).Returns(contacts.GetEnumerator());

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockAppDbContext.Object);

            // Act
            var actual = target.TotalContacts("FirstName2", true);

            // Assert
            Assert.Equal(1, actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(m => m.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<Contact>>().Verify(m => m.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(m => m.ElementType, Times.Exactly(0));
            mockDbSet.As<IQueryable<Contact>>().Verify(m => m.GetEnumerator(), Times.Exactly(0));
        }

        //---------------Get all Paginated Contacts-------------------
        [Fact]
        public void GetPaginatedContactsStartingWithLetter_ReturnsCorrectContacts_WithPagination()
        {
            // Arrange
            var contacts = new List<Contact>
            {
                new Contact { ContactId = 1, FirstName = "Jonnyn", LastName = "Doe", ContactNumber = "1234567890", IsFavourite = false, Country = new Country(), State = new State() },
                new Contact { ContactId = 2, FirstName = "Kane", LastName = "Doe", ContactNumber = "6789012345", IsFavourite = true, Country = new Country(), State = new State() },
                new Contact { ContactId = 3, FirstName = "Lake", LastName = "Smith", ContactNumber = "5432167890", IsFavourite = true, Country = new Country(), State = new State() },
                new Contact { ContactId = 4, FirstName = "Vames", LastName = "Bond", ContactNumber = "0070012345", IsFavourite = false, Country = new Country(), State = new State() }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.ElementType).Returns(contacts.ElementType);
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.GetEnumerator()).Returns(contacts.GetEnumerator());

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetPaginatedContactsStartingWithLetter(1, 2, null, "asc", false);

            // Assert
            Assert.Equal(2, actual.Count());
            Assert.Equal("Jonnyn", actual.First().FirstName);
            Assert.Equal("Kane", actual.Last().FirstName);
            mockDbSet.As<IQueryable<Contact>>().Verify(m => m.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<Contact>>().Verify(m => m.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(m => m.ElementType, Times.Exactly(0));
            mockDbSet.As<IQueryable<Contact>>().Verify(m => m.GetEnumerator(), Times.Exactly(0));
        }

        [Fact]
        public void GetPaginatedContactsStartingWithLetter_ReturnsCorrectContacts_WithSearchString()
        {
            // Arrange
            var contacts = new List<Contact>
            {
               new Contact { ContactId = 1, FirstName = "Jonnyn", LastName = "Doe", ContactNumber = "1234567890", IsFavourite = false, Country = new Country(), State = new State() },
                new Contact { ContactId = 2, FirstName = "Kane", LastName = "Doe", ContactNumber = "6789012345", IsFavourite = true, Country = new Country(), State = new State() },
                new Contact { ContactId = 3, FirstName = "Lake", LastName = "Smith", ContactNumber = "5432167890", IsFavourite = true, Country = new Country(), State = new State() },
                new Contact { ContactId = 4, FirstName = "Vames", LastName = "Bond", ContactNumber = "0070012345", IsFavourite = false, Country = new Country(), State = new State() }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.ElementType).Returns(contacts.ElementType);
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.GetEnumerator()).Returns(contacts.GetEnumerator());

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetPaginatedContactsStartingWithLetter(1, 2, "Jo", "asc", false);

            // Assert
            Assert.Single(actual);
            Assert.Equal("Jonnyn", actual.First().FirstName);
            mockDbSet.As<IQueryable<Contact>>().Verify(m => m.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<Contact>>().Verify(m => m.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(m => m.ElementType, Times.Exactly(0));
            mockDbSet.As<IQueryable<Contact>>().Verify(m => m.GetEnumerator(), Times.Exactly(0));
        }

        [Fact]
        public void GetPaginatedContactsStartingWithLetter_ReturnsCorrectContacts_WithFavourites()
        {
            // Arrange
            var contacts = new List<Contact>
            {
                     new Contact { ContactId = 1, FirstName = "Jonnyn", LastName = "Doe", ContactNumber = "1234567890", IsFavourite = false, Country = new Country(), State = new State() },
                    new Contact { ContactId = 2, FirstName = "Kane", LastName = "Doe", ContactNumber = "6789012345", IsFavourite = true, Country = new Country(), State = new State() },
                    new Contact { ContactId = 3, FirstName = "Lake", LastName = "Smith", ContactNumber = "5432167890", IsFavourite = true, Country = new Country(), State = new State() },
                    new Contact { ContactId = 4, FirstName = "Vames", LastName = "Bond", ContactNumber = "0070012345", IsFavourite = false, Country = new Country(), State = new State() }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.ElementType).Returns(contacts.ElementType);
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.GetEnumerator()).Returns(contacts.GetEnumerator());

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetPaginatedContactsStartingWithLetter(1, 2, null, "asc", true);

            // Assert
            Assert.Equal(2, actual.Count());
            
            mockDbSet.As<IQueryable<Contact>>().Verify(m => m.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<Contact>>().Verify(m => m.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(m => m.ElementType, Times.Exactly(0));
            mockDbSet.As<IQueryable<Contact>>().Verify(m => m.GetEnumerator(), Times.Exactly(0));
        }

        [Fact]
        public void GetPaginatedContactsStartingWithLetter_ReturnsCorrectContacts_WithSortingAsc()
        {
            // Arrange
            var contacts = new List<Contact>
            {
                    new Contact { ContactId = 1, FirstName = "Jonnyn", LastName = "Doe", ContactNumber = "1234567890", IsFavourite = false, Country = new Country(), State = new State() },
                    new Contact { ContactId = 2, FirstName = "Kane", LastName = "Doe", ContactNumber = "6789012345", IsFavourite = true, Country = new Country(), State = new State() },
                    new Contact { ContactId = 3, FirstName = "Lake", LastName = "Smith", ContactNumber = "5432167890", IsFavourite = true, Country = new Country(), State = new State() },
                    new Contact { ContactId = 4, FirstName = "Vames", LastName = "Bond", ContactNumber = "0070012345", IsFavourite = false, Country = new Country(), State = new State() }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.ElementType).Returns(contacts.ElementType);
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.GetEnumerator()).Returns(contacts.GetEnumerator());

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetPaginatedContactsStartingWithLetter(1, 4, null, "asc", false);

            // Assert
            Assert.Equal("Jonnyn", actual.First().FirstName);
           
            Assert.Equal("Vames", actual.Last().FirstName);
            

            mockDbSet.As<IQueryable<Contact>>().Verify(m => m.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<Contact>>().Verify(m => m.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(m => m.ElementType, Times.Exactly(0));
            mockDbSet.As<IQueryable<Contact>>().Verify(m => m.GetEnumerator(), Times.Exactly(0));
        }
        [Fact]
        public void GetPaginatedContactsStartingWithLetter_ReturnsCorrectContacts_WithSortingDesc()
        {
            // Arrange
            var contacts = new List<Contact>
            {
                    new Contact { ContactId = 1, FirstName = "Jonnyn", LastName = "Doe", ContactNumber = "1234567890", IsFavourite = false, Country = new Country(), State = new State() },
                    new Contact { ContactId = 2, FirstName = "Kane", LastName = "Doe", ContactNumber = "6789012345", IsFavourite = true, Country = new Country(), State = new State() },
                    new Contact { ContactId = 3, FirstName = "Lake", LastName = "Smith", ContactNumber = "5432167890", IsFavourite = true, Country = new Country(), State = new State() },
                    new Contact { ContactId = 4, FirstName = "Vames", LastName = "Bond", ContactNumber = "0070012345", IsFavourite = false, Country = new Country(), State = new State() }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.ElementType).Returns(contacts.ElementType);
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.GetEnumerator()).Returns(contacts.GetEnumerator());

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockAppDbContext.Object);

            // Act
            
            var actual = target.GetPaginatedContactsStartingWithLetter(1, 4, null, "desc", false);

            // Assert
            
            Assert.Equal("Vames", actual.First().FirstName);
            Assert.Equal("Jonnyn", actual.Last().FirstName);
           

            mockDbSet.As<IQueryable<Contact>>().Verify(m => m.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<Contact>>().Verify(m => m.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(m => m.ElementType, Times.Exactly(0));
            mockDbSet.As<IQueryable<Contact>>().Verify(m => m.GetEnumerator(), Times.Exactly(0));
        }

        [Fact]
        public void GetPaginatedContactsStartingWithLetter_ReturnsCorrectContacts_WithSorting()
        {
            // Arrange
            var contacts = new List<Contact>
            {
                    new Contact { ContactId = 1, FirstName = "Jonnyn", LastName = "Doe", ContactNumber = "1234567890", IsFavourite = false, Country = new Country(), State = new State() },
                    new Contact { ContactId = 2, FirstName = "Kane", LastName = "Doe", ContactNumber = "6789012345", IsFavourite = true, Country = new Country(), State = new State() },
                    new Contact { ContactId = 3, FirstName = "Lake", LastName = "Smith", ContactNumber = "5432167890", IsFavourite = true, Country = new Country(), State = new State() },
                    new Contact { ContactId = 4, FirstName = "Vames", LastName = "Bond", ContactNumber = "0070012345", IsFavourite = false, Country = new Country(), State = new State() }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.ElementType).Returns(contacts.ElementType);
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.GetEnumerator()).Returns(contacts.GetEnumerator());

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockAppDbContext.Object);

            // Act

            var actual = target.GetPaginatedContactsStartingWithLetter(1, 4, null, "default", false);

            // Assert
            Assert.Equal(4, actual.Count());
            Assert.Equal(1, actual.ElementAt(0).ContactId);
            Assert.Equal(2, actual.ElementAt(1).ContactId);
            Assert.Equal(3, actual.ElementAt(2).ContactId);
            Assert.Equal(4, actual.ElementAt(3).ContactId);


            mockDbSet.As<IQueryable<Contact>>().Verify(m => m.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<Contact>>().Verify(m => m.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(m => m.ElementType, Times.Exactly(0));
            mockDbSet.As<IQueryable<Contact>>().Verify(m => m.GetEnumerator(), Times.Exactly(0));
        }
    }
}
