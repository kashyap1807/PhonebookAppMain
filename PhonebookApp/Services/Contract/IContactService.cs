using PhonebookApp.Models;

namespace PhonebookApp.Services.Contract
{
    public interface IContactService
    {
        int TotalContacts();

        IEnumerable<Contact> GetPaginatedCategories(int page, int pageSize);

        IEnumerable<Contact> GetAllContact();

        Contact? GetContact(int id);

        string AddContact(Contact contact, IFormFile file);

        string ModifyContact(Contact contact);

        string RemoveContact(int id);

    }
}
