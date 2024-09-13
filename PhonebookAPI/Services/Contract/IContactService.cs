using PhonebookAPI.Dtos;
using PhonebookAPI.Models;

namespace PhonebookAPI.Services.Contract
{
    public interface IContactService
    {


        //ServiceResponse<IEnumerable<ContactDto>> GetAllContact();
        ServiceResponse<IEnumerable<AllContactSP>> GetAllContactSP();
        ServiceResponse<IEnumerable<GetAllContactByCountryDto>> GetAllContactByCountry();

        ServiceResponse<IEnumerable<GetAllContactByGenderDto>> GetAllContactByGender();

        ServiceResponse<IEnumerable<GetAllContactByStateIdDto>> GetAllContactByStateId(int stateId);

        ServiceResponse<IEnumerable<GetAllContactByMonthDto>> GetAllContactByMonth(int month);

        ServiceResponse<ContactDto> GetContactById(int id);

        ServiceResponse<string> AddContact(Contact contact);

        ServiceResponse<string> ModifyContact(UpdateContactDto contact);

        ServiceResponse<string> RemoveContact(int id);

        ServiceResponse<IEnumerable<ContactDto>> GetPaginatedContactsStartingWithLetter(int page, int pageSize, string? search_string, string sort_name, bool show_favourites);

        ServiceResponse<int> TotalContacts(string? search_string, bool show_favourites);
        //ServiceResponse<int> TotalContactsStartingWithLetter(char? ch);


    }
}
