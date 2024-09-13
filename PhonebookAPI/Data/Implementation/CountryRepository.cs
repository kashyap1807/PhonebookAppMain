using PhonebookAPI.Data.Contract;
using PhonebookAPI.Models;

namespace PhonebookAPI.Data.Implementation
{
    public class CountryRepository : ICountryRepository
    {
        private IAppDbContext _appDbContext;

        public CountryRepository(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Country> GetAllCountry()
        {
            List<Country> c = _appDbContext.Countries.ToList();
            return c;
        }

        public Country? GetCountryById(int id)
        {
            var c = _appDbContext.Countries.FirstOrDefault(c=>c.CountryId == id);
            return c;
        }
    }
}
