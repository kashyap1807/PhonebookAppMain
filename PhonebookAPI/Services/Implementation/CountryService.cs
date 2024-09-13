using PhonebookAPI.Data.Contract;
using PhonebookAPI.Dtos;
using PhonebookAPI.Services.Contract;

namespace PhonebookAPI.Services.Implementation
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;

        public CountryService(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public ServiceResponse<IEnumerable<CountryDto>> GetAllCountry()
        {
            var response = new ServiceResponse<IEnumerable<CountryDto>>();
            try
            {
                var countries = _countryRepository.GetAllCountry();

                if (countries != null && countries.Any())
                {
                    List<CountryDto> countryDtoList = new List<CountryDto>();
                    foreach (var country in countries)
                    {
                        CountryDto countryDto = new CountryDto();
                        countryDto.CountryId = country.CountryId;
                        countryDto.CountryName = country.CountryName;

                        countryDtoList.Add(countryDto);
                    }

                    response.Data = countryDtoList;
                    response.Success = true;
                    response.Message = "Success";
                }
                else
                {
                    response.Success = false;
                    response.Message = "No record found";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            
            return response;

        }

        public ServiceResponse<CountryDto> GetCoutryById(int id)
        {
            var response = new ServiceResponse<CountryDto>();
            try
            {
                var existingCountry = _countryRepository.GetCountryById(id);

                if (existingCountry != null)
                {
                    var country = new CountryDto()
                    {
                        CountryId = existingCountry.CountryId,
                        CountryName = existingCountry.CountryName,

                    };
                    response.Data = country;
                }
                else
                {
                    response.Success = false;
                    response.Message = "No record found ! ";
                }
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            
            return response;
        }
    }
}
