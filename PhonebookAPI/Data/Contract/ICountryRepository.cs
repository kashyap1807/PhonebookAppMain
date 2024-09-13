using PhonebookAPI.Models;

namespace PhonebookAPI.Data.Contract
{
    public interface ICountryRepository
    {
        IEnumerable<Country> GetAllCountry();

        Country? GetCountryById(int id);
    }
}
