using PhonebookAPI.Dtos;

namespace PhonebookAPI.Services.Contract
{
    public interface ICountryService
    {
        ServiceResponse<IEnumerable<CountryDto>> GetAllCountry();

        ServiceResponse<CountryDto> GetCoutryById(int id);
    }
}
