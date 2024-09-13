using PhonebookAPI.Dtos;
using PhonebookAPI.Models;

namespace PhonebookAPI.Data.Contract
{
    public interface IContactRepository
    {
        //IEnumerable<Contact> GetAll();

        Contact? GetContact(int id);

        bool ContactExists(string num);

        bool ContactExists(int contactId, string num);

        bool InsertContact(Contact c);

        bool UpdateContact(Contact contacts);

        bool DeleteContact(int contactId);

        IEnumerable<Contact> GetPaginatedContactsStartingWithLetter(int page, int pageSize, string? search_string, string sort_name, bool show_favourites);

        int TotalContacts(string? search_string, bool show_favourites);
        //int TotalContactsStartingWithLetter(char? ch);

        //Procedure
        IEnumerable<AllContactSP> GetAllContactSP();

        IEnumerable<GetAllContactByCountryDto> GetAllContactByCountry();

        IEnumerable<GetAllContactByGenderDto> GetAllContactByGender();

        IEnumerable<GetAllContactByStateIdDto> GetAllContactByStateId(int stateId);

        IEnumerable<GetAllContactByMonthDto> GetAllContactByMonth(int month);
    }
}
