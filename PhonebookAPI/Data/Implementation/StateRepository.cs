using Microsoft.EntityFrameworkCore;
using PhonebookAPI.Data.Contract;
using PhonebookAPI.Models;

namespace PhonebookAPI.Data.Implementation
{
    public class StateRepository : IStateRepository
    {
        private IAppDbContext _appDbContext;

        public StateRepository(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        

        public IEnumerable<State> GetStatesBytCountryId(int id)
        {
            var s = _appDbContext.States.Include(s=>s.Country).Where(s=>s.CountryId == id).ToList();
            return s;
        }


    }
}
