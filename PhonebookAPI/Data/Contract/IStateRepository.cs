using PhonebookAPI.Models;

namespace PhonebookAPI.Data.Contract
{
    public interface IStateRepository
    {
        IEnumerable<State> GetStatesBytCountryId(int id);


    }
}
