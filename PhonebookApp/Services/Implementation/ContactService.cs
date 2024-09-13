using PhonebookApp.Data.Contract;
using PhonebookApp.Models;
using PhonebookApp.Services.Contract;

namespace PhonebookApp.Services.Implementation
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public int TotalContacts()
        {
            return _contactRepository.TotalContacts();
        }

        public IEnumerable<Contact> GetPaginatedCategories(int page, int pageSize)
        {
            var c = _contactRepository.GetPaginatedCategories(page, pageSize);
            if (c != null && c.Any())
            {
                foreach (var category in c.Where(c => c.FileName == string.Empty))
                {
                    category.FileName = "DefaultImg.png";
                }
                return c;
            }
            return new List<Contact>();

            //return _categoryRepository.GetPaginatedCategories(page, pageSize);
        }

        public IEnumerable<Contact> GetAllContact()
        {
            var c = _contactRepository.GetAll();
            if (c != null && c.Any())
            {
                foreach (var contact in c.Where(c => c.FileName == string.Empty))
                {
                    contact.FileName = "DefaultImg.png";
                }
                return c;
            }
            return new List<Contact>();
        }

        public Contact? GetContact(int id)
        {
            var c = _contactRepository.GetContact(id);
            return c;
        }

        public string AddContact(Contact contact, IFormFile file)
        {
            if (_contactRepository.ContactExists(contact.ContactNumber))
            {
                return "Contact already exists.";
            }

            var fileName = string.Empty;
            if (file != null && file.Length > 0)
            {
                // Process the uploaded file(eq. save it to disk)
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", file.FileName);

                // Save the file to storage and set path
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    fileName = file.FileName;
                }
                contact.FileName = fileName;
            }

            var result = _contactRepository.InsertContact(contact);

            return result ? "Contact saved successfully." : "Something went wrong, please try after sometime.";
        }

        public string ModifyContact(Contact contact)
        {
            var message = string.Empty;
            if (_contactRepository.ContactExists(contact.contactId, contact.ContactNumber))
            {
                message = "Contact already exists.";
                return message;
            }

            var existingContact = _contactRepository.GetContact(contact.contactId);
            var result = false;
            if (existingContact != null)
            {
                existingContact.FirstName = contact.FirstName;
                existingContact.LastName = contact.LastName;
                existingContact.Email = contact.Email;
                existingContact.Company = contact.Company;
                existingContact.ContactNumber = contact.ContactNumber;

                result = _contactRepository.UpdateContact(existingContact);
            }

            message = result ? "Contact updated successfully." : "Something went wrong, please try after sometime.";
            return message;
        }

        public string RemoveContact(int id)
        {
            var result = _contactRepository.DeleteContact(id);
            if (result)
            {
                return "Contact deleted successfully.";
            }
            else
            {
                return "Something went wrong, please try after sometimes.";
            }
        }



    }
}
